using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRiceMill.Application.Heads.Models;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize]
    public class HeadController : BaseController
    {
        [HttpGet("GetAllHeads")]
        public async Task<IActionResult> GetAllHeads([FromQuery]GetHead1RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPost("Head1")]
        public async Task<IActionResult> CreateHead1(CreateHead1RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPost("Head2")]
        public async Task<IActionResult> CreateHead2(CreateHead2RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPost("Head3")]
        public async Task<IActionResult> CreateHead3(CreateHead3RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPost("Head4")]
        public async Task<IActionResult> CreateHead4(CreateHead4RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPost("Head5")]
        public async Task<IActionResult> CreateHead5(CreateHead5RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPut("Head1")]
        public async Task<IActionResult> UpdateHead1(UpdateHead1RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPut("Head2")]
        public async Task<IActionResult> UpdateHead2(UpdateHead2RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPut("Head3")]
        public async Task<IActionResult> UpdateHead3(UpdateHead3RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPut("Head4")]
        public async Task<IActionResult> UpdateHead4(UpdateHead4RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        [HttpPut("Head5")]
        public async Task<IActionResult> UpdateHead5(UpdateHead5RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}
