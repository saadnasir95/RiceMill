using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Purchases.Commands.DeletePurchase;
using TheRiceMill.Application.Purchases.Commands.UpdatePurchase;
using TheRiceMill.Application.Purchases.Models;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize(Roles = RoleNames.Admin)]
    public class PurchaseController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePurchase(UpdatePurchaseRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases([FromQuery]GetPurchaseRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete]
        public async Task<IActionResult> DeletePurchase([FromQuery]DeletePurchaseRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

    }
}
