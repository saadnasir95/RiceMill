using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Ledgers.Queries.GetCompanyLedger
{
    public class GetCompanyLedgerRequestModel : IRequest<ResponseViewModel>
    {
        public int LedgerType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public CompanyType CompanyId { get; set; }
        public void SetDefaultValue()
        {
        }
    }
}
