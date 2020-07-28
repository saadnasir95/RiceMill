using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize(Roles = RoleNames.Admin)]
    public class PartyController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateParty(CreatePartyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateParty(UpdatePartyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetParty([FromQuery]GetPartyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteParty([FromRoute]DeletePartyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("comboBox")]
        [Authorize]
        public async Task<IActionResult> GetPartyForComboBox([FromQuery]GetPartyForComboBoxRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}