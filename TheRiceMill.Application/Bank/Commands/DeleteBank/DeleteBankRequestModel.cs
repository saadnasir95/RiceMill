using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Bank.Commands.DeleteBank
{

    public class DeleteBankRequestModel : IRequest<ResponseViewModel>
    {
        public int BankId { get; set; }
    }
}