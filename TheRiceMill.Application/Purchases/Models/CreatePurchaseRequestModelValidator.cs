using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Purchases.Models
{
    public class CreatePurchaseRequestModelValidator : AbstractValidator<CreatePurchaseRequestModel>
    {
        public CreatePurchaseRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).Must(p => p > 0).When(p => p.Company == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.ProductId).Must(p => p > 0).When(p => p.Product == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.VehicleId).Must(p => p > 0).When(p => p.Vehicle == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.BagWeight).Required();
            RuleFor(p => p.TotalMaund).Required();
            RuleFor(p => p.KandaWeight).Required();
            RuleFor(p => p.BagQuantity).Required();
            RuleFor(p => p.ExpectedBagWeight).Required();
            RuleFor(p => p.TotalExpectedBagWeight).Required();
            RuleFor(p => p.ExpectedEmptyBagWeight).Required();
            RuleFor(p => p.TotalExpectedEmptyBagWeight).Required();
            RuleFor(p => p.ActualBagWeight).Required();
            RuleFor(p => p.Vibration).Required();
            RuleFor(p => p.TotalActualBagWeight).Required();
            RuleFor(p => p.RatePerKg).Required();
            RuleFor(p => p.RatePerMaund).Required();
            RuleFor(p => p.BasePrice).Required();
            RuleFor(p => p.TotalPrice).Required();
            RuleFor(p => p.ActualBags).Required();
            RuleFor(p => p.Vibration).Required();
            RuleFor(p => p.CheckIn).Required();
            RuleFor(p => p.Direction).IsInEnum();
        }
    }
}