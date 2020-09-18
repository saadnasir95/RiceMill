using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Lots.Models
{
    public class GetYearRequestModel : IRequest<ResponseViewModel>
    {
    }
}
