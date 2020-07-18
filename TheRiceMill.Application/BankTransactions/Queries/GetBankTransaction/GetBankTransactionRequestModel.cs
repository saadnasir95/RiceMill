using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankTransactions.Queries.GetBankTransaction
{

    public class GetBankTransactionRequestModel : IRequest<ResponseViewModel>
    {
        public int BankAccountId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }


        public void SetDefaultValue()
        {
            
        }
    }
}