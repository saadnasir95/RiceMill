using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRiceMill.Application.Heads.Models;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class Head1Controller : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetHead1([FromQuery]GetHead1RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHead1(CreateHead1RequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}
