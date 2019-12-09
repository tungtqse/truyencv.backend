using AutoMapper;
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
using AutoMapper.QueryableExtensions;

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ReadingStory
{
    public class GetListApi
    {
        public class Query : PagingModel, IRequest<Result>
        {
        }

        public class Result : ISearchResult<NestedModel.ChapterModel>, IWebApiResponse
        {
            public IList<NestedModel.ChapterModel> Data { get; set; }
            public int Count { get; set; }
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
                public Chapter Chapter { get; set; }
                public Story Story { get; set; }
                public Author Author { get; set; }
            }

            public class ChapterModel
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public int NumberChapter { get; set; }
                public Guid StoryId { get; set; }
                public string Story { get; set; }
                public Guid AuthorId { get; set; }
                public string Author { get; set; }
                public DateTime ModifiedDate { get; set; }
                public int TotalChapter { get; set; }
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.QueryModel, NestedModel.ChapterModel>()
                    .ForMember(m => m.Id, o => o.MapFrom(f => f.Chapter.Id))
                    .ForMember(m => m.Title, o => o.MapFrom(f => f.Chapter.Title))
                    .ForMember(m => m.NumberChapter, o => o.MapFrom(f => f.Chapter.NumberChapter))
                    .ForMember(m => m.StoryId, o => o.MapFrom(f => f.Chapter.StoryId))
                    .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => f.Chapter.ModifiedDate))
                    .ForMember(m => m.Story, o => o.MapFrom(f => f.Story.Name))
                    .ForMember(m => m.AuthorId, o => o.MapFrom(f => f.Author.Id))
                    .ForMember(m => m.Author, o => o.MapFrom(f => f.Author.Name))
                    .ForMember(m => m.TotalChapter, o => o.MapFrom(f => f.Story.TotalChapter))
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
                    var context = scope.DbContexts.Get<MainContext>();

                    var query = from reading in context.Set<Reading>()
                                join story in context.Set<Story>() on reading.StoryId equals story.Id
                                join chapter in context.Set<Chapter>() on reading.ChapterId equals chapter.Id
                                where reading.StatusId
                                select new NestedModel.QueryModel()
                                {
                                    Chapter = chapter,
                                    Story = story,
                                    Author = story.Author
                                };
                  
                    var count = query.Count();
                    var items = query.OrderByDescending(o => o.Chapter.NumberChapter.Value)
                                     .Skip(message.Skip)
                                     .Take(message.Take)
                                     .ProjectTo<NestedModel.ChapterModel>().ToList();

                    var result = new Result()
                    {
                        Count = count,
                        Data = items,
                        IsSuccessful = true
                    };

                    return Task.FromResult(result);
                }
            }
        }

        #endregion

    }
}
