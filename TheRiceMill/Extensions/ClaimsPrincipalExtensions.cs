using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Presentation.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            string userId = "";
            userId = claimsPrincipal.FindFirstValue(CustomClaimTypes.UserId);
            return userId;
        }
    }
}
