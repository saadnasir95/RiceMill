using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.DeleteBankTransaction
{
    public class DeleteBankTransactionRequestModelValidator : AbstractValidator<DeleteBankTransactionRequestModel>
    {
        public DeleteBankTransactionRequestModelValidator()
        {
            RuleFor(p => p.BankTransactionId).Required();
        }
    }
}