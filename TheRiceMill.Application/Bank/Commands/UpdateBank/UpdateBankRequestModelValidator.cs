using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Bank.Commands.UpdateBank
{
    public class UpdateBankRequestModelValidator : AbstractValidator<UpdateBankRequestModel>
    {
        public UpdateBankRequestModelValidator()
        {
            RuleFor(p => p.BankId).Required();
            RuleFor(p => p.Name).Required();
        }
    }
}