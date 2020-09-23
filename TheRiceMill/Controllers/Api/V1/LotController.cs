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

        [HttpGet("Years")]
        public async Task<IActionResult> GetYears([FromQuery]GetYearRequestModel model)
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


        [HttpPost("RateCost")]
        public async Task<IActionResult> CreateRateCost(CreateRateCostRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut("RateCost")]
        public async Task<IActionResult> UpdateRateCost(UpdateRateCostRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut("ProcessedMaterial")]
        public async Task<IActionResult> UpdateProcessedMaterial(UpdateProcessedMaterialRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}
