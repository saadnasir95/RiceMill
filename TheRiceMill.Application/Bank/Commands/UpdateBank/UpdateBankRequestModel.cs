using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Bank.Commands.UpdateBank
{
    public class UpdateBankRequestModel : IRequest<ResponseViewModel>
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}