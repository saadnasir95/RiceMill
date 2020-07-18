using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankAccount.Commands.CreateBankAccount
{

    public class CreateBankAccountRequestModel : IRequest<ResponseViewModel>
    {
        public string AccountNumber { get; set; }
        public double CurrentBalance { get; set; }
        public int BankId { get; set; }
    }
}