using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{

    [Authorize(Roles = RoleNames.Admin)]
    public class ProductController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]GetProductsRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]DeleteProductRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("comboBox")]
        [Authorize]
        public async Task<IActionResult> GetProductsForComboBox([FromQuery]GetProductsForComboBoxRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}