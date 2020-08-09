using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Ledger.Queries.GetCompanyLedger;
using TheRiceMill.Application.Ledger.Queries.GetLedgerInfo;
using TheRiceMill.Application.Ledger.Queries.GetLedgers;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class LedgerController : BaseController
    {
        [HttpGet("GetPartyLedger")]
        public async Task<IActionResult> GetPartyLedger([FromQuery]GetPartyLedgerRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

        [HttpGet("GetCompanyLedger")]
        public async Task<IActionResult> GetCompanyLedger([FromQuery]GetCompanyLedgerRequestModel model)
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