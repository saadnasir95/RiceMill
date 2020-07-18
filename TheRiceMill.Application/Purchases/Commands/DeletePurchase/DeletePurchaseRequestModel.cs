using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Purchases.Commands.DeletePurchase
{

    public class DeletePurchaseRequestModel : IRequest<ResponseViewModel>
    {
        public int PurchaseId { get; set; }

    }
}