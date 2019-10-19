using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.DbContextScope.Interfaces;
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
    public class SearchApi
    {
        public class Query : PagingModel, IRequest<Result>
        {
            public string Keyword { get; set; }
        }

        public class Result : ISearchResult<NestedModel.StoryModel>, IWebApiResponse
        {
            public IList<NestedModel.StoryModel> Data { get; set; }
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
                public Story Story { get; set; }
                public Author Author { get; set; }
            }

            public class StoryModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public int TotalChapter { get; set; }
                public string Author { get; set; }
                public DateTime ModifiedDate { get; set; }
                public string ModifiedDateDisplay { get; set; }
                public string ProgressStatus { get; set; }
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.QueryModel, NestedModel.StoryModel>()
                    .ForMember(m => m.TotalChapter, o => o.MapFrom(f => f.Story.TotalChapter))
                    .ForMember(m => m.Id, o => o.MapFrom(f => f.Story.Id))
                    .ForMember(m => m.Name, o => o.MapFrom(f => f.Story.Name))
                    .ForMember(m => m.ModifiedDate, o => o.MapFrom(f => f.Story.ModifiedDate))
                    .ForMember(m => m.Author, o => o.MapFrom(f => f.Author.Name))
                    .ForMember(m => m.ProgressStatus, o => o.MapFrom(f => f.Story.ProgressStatus))
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

                    var query = context.Set<Story>()
                            .Where(w => w.StatusId == true);

                    if (!string.IsNullOrEmpty(message.Keyword))
                    {
                        query = query.Where(f => f.Name.Contains(message.Keyword));
                    }

                    var count = query.Count();
                    var items = query.Select(s=> new NestedModel.QueryModel()
                    {
                        Author = s.Author,
                        Story = s
                    }).OrderByDescending(o => o.Story.ModifiedDate).Skip(message.Skip).Take(message.Take).ProjectTo<NestedModel.StoryModel>().ToList();

                    if(items != null && items.Count > 0)
                    {
                        foreach(var item in items)
                        {
                            item.ModifiedDateDisplay = GetModifiedDateDisplay(item.ModifiedDate);
                        }
                    }


                    var result = new Result()
                    {
                        Count = count,
                        Data = items,
                        IsSuccessful = true
                    };

                    return Task.FromResult(result);
                }
            }

            private string GetModifiedDateDisplay(DateTime? modifiedDate)
            {
                if (modifiedDate.HasValue)
                {
                    var now = DateTime.Now;

                    TimeSpan span = now.Subtract(modifiedDate.Value);

                    if (span.Days > 0)
                    {
                        if(span.Days > 365)
                        {
                            return $"{span.Days / 365} year(s) ago";
                        }
                        else if(span.Days > 30)
                        {
                            return $"{span.Days / 30} month(s) ago";
                        }                        

                        return $"{span.Days} day(s) ago";
                    }
                    else
                    {
                        if (span.Hours > 0)
                        {
                            return $"{span.Hours} hour(s) ago";
                        }
                        else if (span.Minutes > 0)
                        {
                            return $"{span.Minutes} minute(s) ago";
                        }

                        return $"{span.Seconds} second(s) ago";
                    }
                }

                return "1 second(s) ago";
            }
        }

        

        #endregion

    }
}
