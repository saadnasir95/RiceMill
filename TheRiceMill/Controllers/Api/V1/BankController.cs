using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Bank.Commands.CreateBank;
using TheRiceMill.Application.Bank.Commands.DeleteBank;
using TheRiceMill.Application.Bank.Commands.UpdateBank;
using TheRiceMill.Application.Bank.Queries.GetAllBanks;
using TheRiceMill.Application.Bank.Queries.GetBanks;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class BankController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateBank(CreateBankRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBank(UpdateBankRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBank([FromQuery]GetBanksRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }


        [HttpDelete("{BankId}")]
        public async Task<IActionResult> DeleteBank([FromRoute]DeleteBankRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("comboBox")]
        [Authorize]
        public async Task<IActionResult> GetBankForComboBox([FromQuery]GetAllBanksRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}