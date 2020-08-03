using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Purchases.Models
{
    public class CreatePurchaseRequestModelValidator : AbstractValidator<CreatePurchaseRequestModel>
    {
        public CreatePurchaseRequestModelValidator()
        {
            RuleFor(p => p.TotalMaund).Required();
            RuleFor(p => p.RatePerMaund).Required();
            RuleFor(p => p.GatepassIds).NotEmpty();
            RuleFor(p => p.TotalPrice).Required();
        }
    }
}