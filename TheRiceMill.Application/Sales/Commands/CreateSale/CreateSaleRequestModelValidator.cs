using System;
using FluentValidation;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Sales.Commands.CreateSale
{
    public class CreateSaleRequestModelValidator : AbstractValidator<CreateSaleRequestModel>
    {
        public CreateSaleRequestModelValidator()
        {
            RuleFor(p => p.TotalMaund).Required();
            RuleFor(p => p.BagQuantity).Required();
            RuleFor(p => p.BagQuantity).Required();
            RuleFor(p => p.Rate).Required();
            RuleFor(p => p.GatepassIds).NotEmpty();
            RuleFor(p => p.TotalPrice).Required();
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}