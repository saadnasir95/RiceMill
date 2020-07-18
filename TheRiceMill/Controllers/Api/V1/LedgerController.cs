using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Ledger.Queries.GetLedgerInfo;
using TheRiceMill.Application.Ledger.Queries.GetLedgers;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class LedgerController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetLedger([FromQuery]GetLedgersRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
        
        [HttpGet("info")]
        public async Task<IActionResult> GetLedgerInfo([FromQuery]GetLedgerInfoRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

    }
}