using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize(Roles = RoleNames.Admin)]
    public class CompanyController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateCompany(CreateCompanyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompany([FromQuery]GetCompanyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute]DeleteCompanyRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("comboBox")]
        [Authorize]
        public async Task<IActionResult> GetCompanyForComboBox([FromQuery]GetCompanyForComboBoxRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}