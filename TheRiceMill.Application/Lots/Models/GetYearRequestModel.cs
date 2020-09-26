using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Lots.Models
{
    public class GetYearRequestModel : IRequest<ResponseViewModel>
    {
        public CompanyType CompanyId { get; set; }
    }
}
