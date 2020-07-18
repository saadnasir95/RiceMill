using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize(Roles = RoleNames.Admin)]
    public class VehicleController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(CreateVehicleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle(UpdateVehicleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicle([FromQuery]GetVehicleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute]DeleteVehicleRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("comboBox")]
        [Authorize]
        public async Task<IActionResult> GetVehicleForComboBox([FromQuery]GetVehicleForComboBoxRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}
