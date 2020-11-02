using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheRiceMill.Application.Voucher.Models;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    public class VoucherController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(CreateVoucherRequestModel model)
        {
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }

    }
}
