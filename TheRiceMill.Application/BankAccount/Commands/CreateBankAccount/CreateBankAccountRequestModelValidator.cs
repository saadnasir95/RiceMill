using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.CreateBankAccount
{
    public class CreateBankAccountRequestModelValidator : AbstractValidator<CreateBankAccountRequestModel>
    {
        public CreateBankAccountRequestModelValidator()
        {
            RuleFor(p => p.AccountNumber).Required();
            RuleFor(p => p.BankId).Required();
            RuleFor(p => p.CurrentBalance).Required();
        }
    }
}