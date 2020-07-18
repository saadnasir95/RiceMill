using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Sale.Commands.DeleteSale
{
    public class DeleteSaleRequestModelValidator : AbstractValidator<DeleteSaleRequestModel>
    {
        public DeleteSaleRequestModelValidator()
        {
            RuleFor(p => p.SaleId).Required();
        }
    }
}