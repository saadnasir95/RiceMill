using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.BankTransactions.Commands.CreateBankTransaction;
using TheRiceMill.Application.BankTransactions.Commands.DeleteBankTransaction;
using TheRiceMill.Application.BankTransactions.Commands.UpdateBankTransaction;
using TheRiceMill.Application.BankTransactions.Queries.GetBankTransaction;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class BankTransactionController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateBankTransaction(CreateBankTransactionRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBankTransaction(UpdateBankTransactionRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBankTransaction([FromQuery]GetBankTransactionRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpDelete("{BankTransactionId}")]
        public async Task<IActionResult> DeleteBankTransaction([FromRoute]DeleteBankTransactionRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        
    }
}