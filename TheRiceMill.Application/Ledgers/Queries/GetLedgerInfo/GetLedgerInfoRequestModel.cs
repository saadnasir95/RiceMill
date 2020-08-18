using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgerInfo
{

    public class GetLedgerInfoRequestModel : IRequest<ResponseViewModel>
    {
        public LedgerType LedgerType { get; set; }
        public int Id { get; set; }
    }
}