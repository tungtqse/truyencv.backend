using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TruyenCV_BackEnd.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChapterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Get(ApplicationApi.APIs.ChapterApi.GetApi.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(ApplicationApi.APIs.ChapterApi.SearchApi.Query query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationApi.APIs.ChapterApi.CreateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationApi.APIs.ChapterApi.UpdateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationApi.APIs.ChapterApi.DeleteApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetBookmark(ApplicationApi.APIs.ChapterApi.CreateBookmarkApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}