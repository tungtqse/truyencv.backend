using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.DbContextScope.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TruyenCV_BackEnd.Common.Models;
using TruyenCV_BackEnd.DataAccess;
using TruyenCV_BackEnd.DataAccess.Models;
using System.Linq;
using TruyenCV_BackEnd.Utility;

namespace TruyenCV_BackEnd.ApplicationApi.APIs.AuthorApi
{
    public class GetApi
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result : IWebApiResponse
        {
            public NestedModel.AuthorModel Data { get; set; }
            public Result()
            {
                Messages = new List<string>();
            }

            public int Code { get; set; }
            public bool IsSuccessful { get; set; }
            public List<string> Messages { get; set; }
        }

        public class NestedModel
        {
            public class AuthorQueryModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Link { get; set; }

                public ICollection<Story> Stories { get; set; }
            }

            public class AuthorModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Link { get; set; }       
                
                public List<StoryModel> Stories { get; set; }

                public AuthorModel()
                {
                    Stories = new List<StoryModel>();
                }
            }

            public class StoryModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string ProgressStatus { get; set; }
                public int TotalChapter { get; set; }
                public string Link { get; set; }
                public string Source { get; set; }
                public DateTime ModifiedDate { get; set; }
                public string ModifiedDateDisplay { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.AuthorQueryModel, NestedModel.AuthorModel>()
                    .ForMember(m => m.Id, o => o.MapFrom(f => f.Id))
                    .ForMember(m => m.Name, o => o.MapFrom(f => f.Name))
                    .ForMember(m => m.Link, o => o.MapFrom(f => f.Link))
                    ;             

                CreateMap<Story, NestedModel.StoryModel>()
                    .ForMember(m => m.ModifiedDateDisplay, o => o.MapFrom(f => Helper.GetModifiedDateDisplay(f.ModifiedDate)))
                    ;
            }            
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly IDbContextScopeFactory _scopeFactory;

            public QueryHandler(IDbContextScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
            }

            public Task<Result> Handle(Query message, CancellationToken cancellationToken)
            {
                using (var scope = _scopeFactory.CreateReadOnly())
                {
                    var result = new Result();

                    var context = scope.DbContexts.Get<MainContext>();

                    var item = (from author in context.Set<Author>()
                                where author.Id == message.Id
                                select new NestedModel.AuthorQueryModel()
                                {
                                    Id = author.Id,
                                    Name = author.Name,
                                    Link = author.Link,
                                    Stories = author.Stories
                                }).ProjectTo<NestedModel.AuthorModel>().FirstOrDefault();


                    if (item != null)
                    {


                        result.IsSuccessful = true;
                        result.Data = item;
                    }
                    else
                    {
                        result.Messages.Add("Author not found");
                    }

                    return Task.FromResult(result);
                }
            }
        }
    }
}
