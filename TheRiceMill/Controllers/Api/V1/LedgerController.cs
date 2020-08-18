using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRiceMill.Application.Ledgers.Queries.GetCompanyLedger;
using TheRiceMill.Application.Ledgers.Queries.GetLedgerInfo;
using TheRiceMill.Application.Ledgers.Queries.GetLedgers;

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

        [HttpPost("GetCompanyLedger")]
        public async Task<IActionResult> GetCompanyLedger(GetCompanyLedgerRequestModel model)
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