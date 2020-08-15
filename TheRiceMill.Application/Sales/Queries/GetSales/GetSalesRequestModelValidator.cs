using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Sales.Queries.GetSales
{
    public class GetSalesRequestModelValidator : AbstractValidator<GetSalesRequestModel>
    {
        public GetSalesRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
        }
    }
}