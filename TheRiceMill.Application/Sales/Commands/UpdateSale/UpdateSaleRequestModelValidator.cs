using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Sales.Commands.UpdateSale
{
    public class UpdateSaleRequestModelValidator : AbstractValidator<UpdateSaleRequestModel>
    {
        public UpdateSaleRequestModelValidator()
        {
            RuleFor(p => p.TotalMaund).Required();
            RuleFor(p => p.Rate).Required();
            RuleFor(p => p.TotalPrice).Required();
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}