using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRiceMill.Application.Lots.Models;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize]
    public class LotController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetLot([FromQuery]GetLotRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProcessedMaterial(CreateProcessedMaterialRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}
