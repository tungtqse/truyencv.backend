﻿using AutoMapper;
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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.AuthorApi
{
    public class SearchApi
    {
        public class Query : PagingModel, IRequest<Result>
        {
            public string Name { get; set; }
        }

        public class Result : ISearchResult<NestedModel.AuthorModel>, IWebApiResponse
        {
            public IList<NestedModel.AuthorModel> Data { get; set; }
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
            public class AuthorModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Link { get; set; }
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Author, NestedModel.AuthorModel>()
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

                    var query =
                        context.Set<Author>()
                            .Where(w => w.StatusId == true);

                    if (!string.IsNullOrEmpty(message.Name))
                    {
                        query = query.Where(f => f.Name.Contains(message.Name));
                    }

                    var count = query.Count();
                    var items = query.OrderBy(s => s.Name).Skip(message.Skip).Take(message.Take).ProjectTo<NestedModel.AuthorModel>().ToList();

                    var result = new Result()
                    {
                        Count = count,
                        Data = items
                    };

                    return Task.FromResult(result);
                }
            }
        }

        #endregion

    }
}
