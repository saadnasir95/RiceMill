using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server;
using TheRiceMill.Application.Users.Models;
using TheRiceMill.Common.Constants;
using TheRiceMill.Presentation.Extensions;

namespace TheRiceMill.Presentation.Controllers.Api.V1
{
    [Authorize]
    public class UserController : BaseController
    {
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            model.UserId = User.UserId();
            var response = await Mediator.Send(model);
            return StatusCode(response.Status, response.Response);
        }
    }
}