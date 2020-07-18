using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.DeleteBankAccount
{
    public class DeleteBankAccountRequestModelValidator : AbstractValidator<DeleteBankAccountRequestModel>
    {
        public DeleteBankAccountRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
        }
    }
}