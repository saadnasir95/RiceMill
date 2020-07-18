using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.BankAccount.Commands.DeleteBankAccount
{

    public class DeleteBankAccountRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }
}