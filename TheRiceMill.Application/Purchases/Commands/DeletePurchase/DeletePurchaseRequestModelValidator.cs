using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Purchases.Commands.DeletePurchase
{
    public class DeletePurchaseRequestModelValidator : AbstractValidator<DeletePurchaseRequestModel>
    {
        public DeletePurchaseRequestModelValidator()
        {
            RuleFor(p => p.PurchaseId).Required();
        }
    }
}