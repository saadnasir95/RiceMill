using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankTransactions.Commands.DeleteBankTransaction
{

    public class DeleteBankTransactionRequestModel : IRequest<ResponseViewModel>
    {
        public int BankTransactionId { get; set; }
    }
}