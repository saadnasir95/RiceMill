using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Sales.Queries.GetSales
{
    public class GetSalesRequestModelValidator : AbstractValidator<GetSalesRequestModel>
    {
        public GetSalesRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}