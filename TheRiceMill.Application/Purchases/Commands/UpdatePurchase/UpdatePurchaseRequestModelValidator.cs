using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Purchases.Commands.UpdatePurchase
{
    public class UpdatePurchaseRequestModelValidator : AbstractValidator<UpdatePurchaseRequestModel>
    {
        public UpdatePurchaseRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.TotalMaund).Required();
            RuleFor(p => p.Rate).Required();
            RuleFor(p => p.TotalPrice).Required();
            RuleFor(p => p.BasePrice).Required();
            RuleFor(p => p.Freight).Required();
        }
    }
}