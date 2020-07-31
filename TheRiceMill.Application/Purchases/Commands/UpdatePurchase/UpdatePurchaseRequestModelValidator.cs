using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Purchases.Commands.UpdatePurchase
{
    public class UpdatePurchaseRequestModelValidator : AbstractValidator<UpdatePurchaseRequestModel>
    {
        public UpdatePurchaseRequestModelValidator()
        {
            RuleFor(p => p.TotalMaund).Required();
/*            RuleFor(p => p.KandaWeight).Required();
            RuleFor(p => p.BagQuantity).Required();
            RuleFor(p => p.ExpectedBagWeight).Required();
            RuleFor(p => p.TotalExpectedBagWeight).Required();
            RuleFor(p => p.ExpectedEmptyBagWeight).Required();
            RuleFor(p => p.TotalExpectedEmptyBagWeight).Required();
            RuleFor(p => p.ActualBagWeight).Required()*/;
/*            RuleFor(p => p.TotalActualBagWeight).Required();
            RuleFor(p => p.RatePerKg).Required();*/
            RuleFor(p => p.RatePerMaund).Required();
            RuleFor(p => p.TotalPrice).Required();
        }
    }
}