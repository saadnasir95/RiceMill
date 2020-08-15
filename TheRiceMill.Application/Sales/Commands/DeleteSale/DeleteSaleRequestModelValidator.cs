using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Sales.Commands.DeleteSale
{
    public class DeleteSaleRequestModelValidator : AbstractValidator<DeleteSaleRequestModel>
    {
        public DeleteSaleRequestModelValidator()
        {
            RuleFor(p => p.SaleId).Required();
        }
    }
}