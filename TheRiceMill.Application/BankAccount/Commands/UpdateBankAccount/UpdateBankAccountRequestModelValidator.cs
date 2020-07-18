using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.UpdateBankAccount
{
    public class UpdateBankAccountRequestModelValidator : AbstractValidator<UpdateBankAccountRequestModel>
    {
        public UpdateBankAccountRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
            RuleFor(p => p.AccountNumber).Required();
            RuleFor(p => p.BankId).Required();
            RuleFor(p => p.CurrentBalance).Required();

        }
    }
}