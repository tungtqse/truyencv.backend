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
    public class ReadingStoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadingStoryController(IMediator mediator)
        {
            _mediator = mediator;
        }        

        [HttpPost]
        public async Task<IActionResult> Search(ApplicationApi.APIs.ReadingStory.GetListApi.Query query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationApi.APIs.ReadingStory.CreateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationApi.APIs.ReadingStory.DeleteApi.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}