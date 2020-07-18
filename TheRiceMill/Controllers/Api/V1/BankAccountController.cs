using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.BankAccount.Commands.CreateBankAccount;
using TheRiceMill.Application.BankAccount.Commands.DeleteBankAccount;
using TheRiceMill.Application.BankAccount.Commands.UpdateBankAccount;
using TheRiceMill.Application.BankAccount.Queries.GetBankAccounts;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class BankAccountController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(CreateBankAccountRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBankAccount(UpdateBankAccountRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBankAccount([FromQuery]GetBankAccountsRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBankAccount([FromRoute]DeleteBankAccountRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        
    }
}