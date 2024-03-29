using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Application.Sale.Commands.DeleteSale;
using TheRiceMill.Application.Sale.Commands.UpdateSale;
using TheRiceMill.Application.Sale.Queries.GetSales;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize]
    public class SaleController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateSale(CreateSaleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSale(UpdateSaleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSale([FromQuery]GetSalesRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteSale([FromQuery]DeleteSaleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

    }
}