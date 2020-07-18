using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgerInfo
{

    public class GetLedgerInfoRequestModel : IRequest<ResponseViewModel>
    {
        public LedgerType LedgerType { get; set; }
        public int TransactionId { get; set; }
    }
}