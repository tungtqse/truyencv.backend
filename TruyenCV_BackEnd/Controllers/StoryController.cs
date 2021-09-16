using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TruyenCV_BackEnd.Controllers
{

    [Microsoft.AspNetCore.Cors.EnableCors("AllowOrigin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Get(ApplicationApi.APIs.StoryApi.GetApi.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(ApplicationApi.APIs.StoryApi.SearchApi.Query query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationApi.APIs.StoryApi.CreateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationApi.APIs.StoryApi.UpdateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationApi.APIs.StoryApi.DeleteApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}