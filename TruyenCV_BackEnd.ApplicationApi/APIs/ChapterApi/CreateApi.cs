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
using TruyenCV_BackEnd.Common;
using TruyenCV_BackEnd.Common.Models;
using TruyenCV_BackEnd.DataAccess;
using TruyenCV_BackEnd.DataAccess.Models;

namespace TruyenCV_BackEnd.ApplicationApi.APIs.ChapterApi
{
    public class CreateApi
    {
        public class Command : IRequest<CommandResponse>
        {
            public Guid StoryId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Link { get; set; }
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
                RuleFor(f => f.Title).NotNull().NotEmpty()
                    .WithMessage("Title is null or empty");
                RuleFor(f => f.Content).NotNull().NotEmpty()
                    .WithMessage("Content is null or empty");
                RuleFor(f => f.Link).NotNull().NotEmpty()
                    .WithMessage("Link is null or empty");
            }
        }

        // Mapping
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Chapter>()
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

                var isValid = false;

                using (var scope = _scopeFactory.Create())
                {
                    var context = scope.DbContexts.Get<MainContext>();

                    isValid = context.Set<Chapter>().Any(f => f.Title.Equals(message.Title, StringComparison.OrdinalIgnoreCase));

                    if (!isValid)
                    {  
                        var maxChapter = context.Set<Chapter>().Max(f => f.NumberChapter).Value;                      

                        var chapter = Mapper.Map<Chapter>(message);
                        chapter.NumberChapter = maxChapter + 1;

                        context.Set<Chapter>().Add(chapter);

                        var story = context.Set<Story>().FirstOrDefault(f => f.Id == message.StoryId);
                        story.TotalChapter = chapter.NumberChapter.Value;
                        
                        isValid = true;
                    }
                    else
                    {
                        result.Messages.Add("Chapter title was existed");
                    }

                    scope.SaveChanges();
                }

                result.IsSuccessful = isValid;

                #endregion


                return Task.FromResult(result);
            }
        }

        #endregion

    }
}
