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

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ReadingStory
{
    public class CreateApi
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
                    .WithMessage("Story is null or empty");
                RuleFor(f => f.ChapterId).NotEmpty().NotEqual(Guid.Empty)
                    .WithMessage("Chapter is null or empty");
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Reading>()
                    .ForMember(m => m.Id, o => o.MapFrom(f => Guid.NewGuid()))
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

                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<MainContext>();

                    var currentReading = context.Set<Reading>().FirstOrDefault(f => f.StoryId == message.StoryId);

                    if (currentReading != null)
                    {
                        currentReading.ChapterId = message.ChapterId;
                        currentReading.StatusId = true;
                    }
                    else
                    {
                        var reading = new Reading
                        {
                            StoryId = message.StoryId,
                            ChapterId = message.ChapterId
                        };

                        context.Set<Reading>().Add(reading);
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
