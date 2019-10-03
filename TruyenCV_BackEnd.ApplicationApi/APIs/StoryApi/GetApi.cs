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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.StoryApi
{
    public class GetApi
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result : IWebApiResponse
        {
            public NestedModel.StoryModel Data { get; set; }
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
            public class QueryModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string ProgressStatus { get; set; }
                public int TotalChapter { get; set; }
                public string Link { get; set; }
                public string Source { get; set; }
                public string Author { get; set; }

                public ICollection<Chapter> Chapters { get; set; }
            }
            
            public class StoryModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string ProgressStatus { get; set; }
                public int TotalChapter { get; set; }
                public string Link { get; set; }
                public string Source { get; set; }
                public string Author { get; set; }

                public List<ChapterModel> Chapters { get; set; }

                public StoryModel()
                {
                    Chapters = new List<ChapterModel>();
                }
            }

            public class ChapterModel
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public DateTime ModifiedDate { get; set; }
            }            
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.QueryModel, NestedModel.StoryModel>()
                    //.ForMember(m => m.Id, o => o.MapFrom(f => f.Id))
                    //.ForMember(m => m.Name, o => o.MapFrom(f => f.Name))
                    //.ForMember(m => m.Link, o => o.MapFrom(f => f.Link))
                    ;

                CreateMap<Chapter, NestedModel.ChapterModel>()
                    .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => f.ModifiedDate))
                    ;
            }
        }

        #region QueryHandler

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

                    var item = (from story in context.Set<Story>()
                                where story.Id == message.Id
                                select new NestedModel.QueryModel()
                                {
                                    Id = story.Id,
                                    Name = story.Name,
                                    Link = story.Link,
                                    Author = story.Author.Name,
                                    ProgressStatus = story.ProgressStatus,
                                    Source = story.Source,
                                    TotalChapter = story.TotalChapter,
                                    Chapters = story.Chapters.OrderByDescending(o=>o.NumberChapter).ToList()
                                }).ProjectTo<NestedModel.StoryModel>().FirstOrDefault();


                    if (item != null)
                    {
                        result.IsSuccessful = true;
                        result.Data = item;
                    }
                    else
                    {
                        result.Messages.Add("Story not found");
                    }

                    return Task.FromResult(result);
                }
            }
        }

        #endregion
    }
}
