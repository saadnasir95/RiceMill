using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgers
{
    public class GetPartyLedgerRequestModel : IRequest<ResponseViewModel>
    {
        public int PartyId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public CompanyType CompanyId { get; set; }

        public void SetDefaultValue()
        {
            
        }
    }
}