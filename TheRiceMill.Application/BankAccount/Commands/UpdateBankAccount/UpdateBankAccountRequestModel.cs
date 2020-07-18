using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankAccount.Commands.UpdateBankAccount
{

    public class UpdateBankAccountRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public double CurrentBalance { get; set; }
        public int BankId { get; set; }
    }
}