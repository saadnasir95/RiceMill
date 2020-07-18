using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Sale.Commands.DeleteSale
{

    public class DeleteSaleRequestModel : IRequest<ResponseViewModel>
    {
        public int SaleId { get; set; }
    }
}