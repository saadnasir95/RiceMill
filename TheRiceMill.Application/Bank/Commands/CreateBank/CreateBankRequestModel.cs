using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Bank.Commands.CreateBank
{
    public class CreateBankRequestModel : IRequest<ResponseViewModel>
    {
        public string Name { get; set; }
    }
}