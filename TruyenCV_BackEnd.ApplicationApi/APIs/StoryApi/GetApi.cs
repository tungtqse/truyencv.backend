﻿using AutoMapper;
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
                public Guid AuthorId { get; set; }
                public string Description { get; set; }
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
                public Guid AuthorId { get; set; }
                public string Description { get; set; }
            }        
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<NestedModel.QueryModel, NestedModel.StoryModel>()
                    .ForMember(m => m.Description, o => o.MapFrom(f => (!string.IsNullOrEmpty(f.Description) ? f.Description: "")))
                    //.ForMember(m => m.Name, o => o.MapFrom(f => f.Name))
                    //.ForMember(m => m.Link, o => o.MapFrom(f => f.Link))
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
                                    AuthorId = story.AuthorId, 
                                    ProgressStatus = story.ProgressStatus,
                                    Source = story.Source,
                                    TotalChapter = story.TotalChapter,
                                    Description = story.Description
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
