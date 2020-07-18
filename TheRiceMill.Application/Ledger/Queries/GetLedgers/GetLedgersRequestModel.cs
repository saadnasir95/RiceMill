using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgers
{
    public class GetLedgersRequestModel : IRequest<ResponseViewModel>
    {
        public int CompanyId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }


        public void SetDefaultValue()
        {
            
        }
    }
}