using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using TruyenCV_BackEnd.Common.Models;
using TruyenCV_BackEnd.DataAccess;
using TruyenCV_BackEnd.DataAccess.DbScopeFactory;
using TruyenCV_BackEnd.LoggerService;
using TruyenCV_BackEnd.Utility.ErrorHandle;

namespace TruyenCV_BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddFluentValidation(fv =>
                    {
                        fv.RegisterValidatorsFromAssemblyContaining<ApplicationApi.APIModule>();
                        fv.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                        fv.ImplicitlyValidateChildProperties = true;
                    });

            services.AddDbContext<MainContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), builder => builder.MigrationsAssembly(typeof(Startup).Assembly.FullName)));
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddScoped(typeof(ApplicationDbContextOptions));
            services.AddTransient(typeof(IDbContextFactory), typeof(DbContextFactory));
            services.AddTransient(typeof(IDbContextScopeFactory), typeof(DbContextScopeFactory));
            services.AddAutoMapper();

            // register MediatR Handler
            MediatorConfig(services, typeof(ApplicationApi.APIModule), "Handler");

            // register AutoMapper
            AutoMapperConfig(services, typeof(ApplicationApi.APIModule));

            // configure HttpContext
            services.AddHttpContextAccessor();

            // configure FluentValidation
            FluentValidationConfig(services);

            // configure Swagger
            SwaggerConfig(services);

            // configure NLog + Elmah
            services.ConfigureLoggerService();

            // Enable Cors

            services.AddCors(c =>
            {
                //c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
                c.AddPolicy("CorsPolicy", options => options.AllowAnyOrigin()
                                                            .AllowAnyMethod()
                                                            .AllowAnyHeader());
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.WithOrigins("http://localhost:4218/", "http://localhost:3000/");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }           

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());            

            Utility.AppContext.Configure(app.ApplicationServices
                      .GetRequiredService<IHttpContextAccessor>());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // https://localhost:44336/api-doc/index.html
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Core V1");
            });

            // UseExceptionHandler extension
            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AutoMapperConfig(IServiceCollection services, Type type)
        {
            var assemblyToScan = Assembly.GetAssembly(type);
            var assemblies = new[] { assemblyToScan };

            var profiles = assemblies.SelectMany(x => x.GetExportedTypes()
                .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Where(y => y.IsClass && !y.IsAbstract && !y.IsGenericType && y.Name.EndsWith("MappingProfile")));

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        private void MediatorConfig(IServiceCollection services, Type type, string name)
        {
            //Get Get Assembly of service project
            //var assemblyToScan = Assembly.GetAssembly(type); //..or whatever assembly you need
            var assemblyToScan = Assembly.GetAssembly(type);
            var assemblies = new[] { assemblyToScan };

            var allPublicTypes = assemblies.SelectMany(x => x.GetExportedTypes()
                .Where(y => y.IsClass && !y.IsAbstract && !y.IsGenericType && y.Name.EndsWith(name)));

            //Config services injection
            foreach (var handler in allPublicTypes)
            {
                services.AddMediatR(handler.GetTypeInfo().Assembly);
            }
        }

        private void FluentValidationConfig(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(m => m.ErrorMessage)).ToList();
                    var result = new ErrorDetails
                    {
                        Code = 500,
                        State = "Internal Server Error",
                        IsSuccessful = false,
                        Messages = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
        }

        private void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Web API Core", Version = "v1" });
                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{
                //    { "Bearer", new string[] { } }
                //});
                // UseFullTypeNameInSchemaIds replacement for .NET Core when duplicate model name
                c.CustomSchemaIds(x => x.FullName);
                c.AddFluentValidationRules();
            });
        }
    }
}
