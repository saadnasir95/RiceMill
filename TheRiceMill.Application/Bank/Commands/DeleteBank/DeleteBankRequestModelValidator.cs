using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Bank.Commands.DeleteBank
{
    public class DeleteBankRequestModelValidator : AbstractValidator<DeleteBankRequestModel>
    {
        public DeleteBankRequestModelValidator()
        {
            RuleFor(p => p.BankId).Required();
        }
    }
}