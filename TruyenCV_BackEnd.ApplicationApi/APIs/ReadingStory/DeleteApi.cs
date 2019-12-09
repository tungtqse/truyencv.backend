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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ReadingStory
{
    public class DeleteApi
    {
        public class Command : IRequest<CommandResponse>
        {
            public Guid StoryId { get; set; }
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
                    .WithMessage("Story is null or empty");
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
                var isValid = false;

                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<MainContext>();

                    var currentReading = context.Set<Reading>().FirstOrDefault(f => f.StoryId == message.StoryId);

                    if (currentReading != null)
                    {
                        currentReading.StatusId = false;
                        isValid = true;
                    }

                    scope.SaveChanges();
                }

                result.IsSuccessful = isValid;


                return Task.FromResult(result);
            }
        }

        #endregion
    }
}
