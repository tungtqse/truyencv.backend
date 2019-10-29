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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ChapterApi
{
    public class CreateBookmarkApi
    {
        public class Command : IRequest<CommandResponse>
        {
            public Guid StoryId { get; set; }
            public Guid ChapterId { get; set; }
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
                RuleFor(f => f.StoryId).NotEmpty().NotEqual(Guid.Empty)
                    .WithMessage("Story Id is null or empty");
                RuleFor(f => f.ChapterId).NotEmpty().NotEqual(Guid.Empty)
                    .WithMessage("Chapter Id is null or empty");
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

                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<MainContext>();

                    var currentBookmark  = context.Set<Bookmark>().FirstOrDefault(f => f.StoryId == message.StoryId && f.ChapterId == message.ChapterId);

                    if (currentBookmark != null)
                    {
                        currentBookmark.StatusId = !currentBookmark.StatusId;
                    }
                    else
                    {
                        var bookmark = new Bookmark
                        {
                            StoryId = message.StoryId,
                            ChapterId = message.ChapterId
                        };

                        context.Set<Bookmark>().Add(bookmark);
                    }

                    scope.SaveChanges();
                }

                result.IsSuccessful = true;

                #endregion


                return Task.FromResult(result);
            }
        }

        #endregion
    }
}
