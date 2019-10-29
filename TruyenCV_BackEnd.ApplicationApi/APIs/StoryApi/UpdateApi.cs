using AutoMapper;
using EntityFramework.DbContextScope.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TruyenCV_BackEnd.Common.Models;
using TruyenCV_BackEnd.DataAccess;
using TruyenCV_BackEnd.DataAccess.Models;

namespace TruyenCV_BackEnd.ApplicationApi.APIs.StoryApi
{
    public class UpdateApi
    {
        public class Command : IRequest<CommandResponse>
        {
            public Guid Id { get; set; }
            public Guid AuthorId { get; set; }
            public string Name { get; set; }
            public string ProgressStatus { get; set; }
            public string Link { get; set; }
            public string Source { get; set; }
            public string Description { get; set; }
        }

        public class CommandResponse : IWebApiResponse
        {
            public CommandResponse()
            {
                Messages = new List<string>();
            }

            public int Code { get; set; }
            public bool IsSuccessful { get; set; }
            public List<string> Messages { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(f => f.AuthorId).NotEmpty().NotEqual(Guid.Empty)
                    .WithMessage("Author is null or empty");
                RuleFor(f => f.Name).NotNull().NotEmpty()
                    .WithMessage("Name is null or empty");
                RuleFor(f => f.Link).NotNull().NotEmpty()
                    .WithMessage("Link is null or empty");
                RuleFor(f => f.Source).NotNull().NotEmpty()
                    .WithMessage("Source is null or empty");
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Story>()
                    .ForMember(m => m.TotalChapter, o => o.Ignore())
                    ;
            }
        }

        #region CommandHandler

        public class CommandHandler : IRequestHandler<Command, CommandResponse>
        {
            private readonly IDbContextScopeFactory _scopeFactory;
            private readonly IValidatorFactory _validatorFactory;

            public CommandHandler(IDbContextScopeFactory scopeFactory, IValidatorFactory validatorFactory)
            {
                _scopeFactory = scopeFactory;
                _validatorFactory = validatorFactory;
            }

            public Task<CommandResponse> Handle(Command message, CancellationToken cancellationToken)
            {
                var result = new CommandResponse();

                #region Validate

                var isValid = false;

                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<MainContext>();

                    isValid = context.Set<Story>().Any(f => f.Id != message.Id && f.Name.Equals(message.Name, StringComparison.OrdinalIgnoreCase));

                    if (!isValid)
                    {
                        var story = context.Set<Story>().FirstOrDefault(f => f.Id == message.Id);

                        if (story != null)
                        {
                            story = Mapper.Map(message, story);
                            isValid = true;
                        }
                        else
                        {
                            result.Messages.Add("Not found Story");
                        }
                    }
                    else
                    {
                        result.Messages.Add("Story name was existed");
                    }

                    scope.SaveChanges();
                }

                result.IsSuccessful = isValid;

                #endregion


                return Task.FromResult(result);
            }
        }

        #endregion
    }
}
