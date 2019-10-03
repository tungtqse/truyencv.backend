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
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Get(ApplicationApi.APIs.AuthorApi.GetApi.Query query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(ApplicationApi.APIs.AuthorApi.SearchApi.Query query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationApi.APIs.AuthorApi.CreateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationApi.APIs.AuthorApi.UpdateApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationApi.APIs.AuthorApi.DeleteApi.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}