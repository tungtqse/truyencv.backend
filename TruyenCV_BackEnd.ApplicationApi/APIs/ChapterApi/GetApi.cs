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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ChapterApi
{
    public class GetApi
    {
        public class Query : IRequest<Result>
        {
            public Guid? Id { get; set; }
            public int? NumberChapter { get; set; }
            public Guid? StoryId { get; set; }
        }

        public class Result : IWebApiResponse
        {
            public NestedModel.ChapterModel Data { get; set; }
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
                public Bookmark Bookmark { get; set; }
            }

            public class ChapterModel
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
                public int NumberChapter { get; set; }
                public Guid StoryId { get; set; }
                public string Story { get; set; }
                public Guid AuthorId { get; set; }
                public string Author { get; set; }
                public DateTime ModifiedDate { get; set; }
                public int NextNumberChapter { get; set; }
                public int PreNumberChapter { get; set; }
                public bool IsBookmark { get; set; }
            }       
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.QueryModel, NestedModel.ChapterModel>()
                    .ForMember(m => m.Id, o => o.MapFrom(f => f.Chapter.Id))
                    .ForMember(m => m.Title, o => o.MapFrom(f => f.Chapter.Title))
                    .ForMember(m => m.Content, o => o.MapFrom(f => f.Chapter.Content))
                    .ForMember(m => m.NumberChapter, o => o.MapFrom(f => f.Chapter.NumberChapter))
                    .ForMember(m => m.StoryId, o => o.MapFrom(f => f.Chapter.StoryId))
                    .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => f.Chapter.ModifiedDate))
                    .ForMember(m => m.Story, o => o.MapFrom(f => f.Story.Name))
                    .ForMember(m => m.AuthorId, o => o.MapFrom(f => f.Author.Id))
                    .ForMember(m => m.Author, o => o.MapFrom(f => f.Author.Name))
                    .ForMember(m => m.NextNumberChapter, o => o.MapFrom(f => GetNextNumberChapter(f.Chapter.NumberChapter.Value, f.Story.TotalChapter)))
                    .ForMember(m => m.PreNumberChapter, o => o.MapFrom(f => GetPreNumberChapter(f.Chapter.NumberChapter.Value)))
                    .ForMember(m => m.IsBookmark, o => o.MapFrom(f => (f.Bookmark != null && f.Bookmark.ChapterId == f.Chapter.Id && f.Bookmark.StatusId)))
                    ;
            }

            private int GetNextNumberChapter(int currentNumberChapter, int totalNumberChapter)
            {
                return (currentNumberChapter < totalNumberChapter) ? (currentNumberChapter + 1) : -1;
            }

            private int GetPreNumberChapter(int currentNumberChapter)
            {
                return (currentNumberChapter > 1) ? (currentNumberChapter - 1) : -1;
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

                    var queryChapter = context.Set<Chapter>().Where(f=>f.StatusId);

                    if (message.Id.HasValue)
                    {
                        queryChapter = queryChapter.Where(f => f.Id == message.Id.Value);
                    }

                    if (message.NumberChapter.HasValue)
                    {
                        queryChapter = queryChapter.Where(f => f.NumberChapter == message.NumberChapter);
                    }

                    if (message.StoryId.HasValue)
                    {
                        queryChapter = queryChapter.Where(f => f.StoryId == message.StoryId.Value);
                    }                   

                    var item = ( from chapter in queryChapter
                                 join bookmark in context.Set<Bookmark>() on chapter.Id equals bookmark.ChapterId into bmJoin
                                 from bookmark in bmJoin.DefaultIfEmpty()                               
                                select new NestedModel.QueryModel()
                                {
                                    Chapter = chapter,
                                    Story = chapter.Story,
                                    Author = chapter.Story.Author,
                                    Bookmark = bookmark
                                }).ProjectTo<NestedModel.ChapterModel>().FirstOrDefault();


                    if (item != null)
                    {
                        result.IsSuccessful = true;
                        result.Data = item;
                    }
                    else
                    {
                        result.Messages.Add("Chapter not found");
                    }

                    return Task.FromResult(result);
                }
            }
        }

        #endregion
    }
}
