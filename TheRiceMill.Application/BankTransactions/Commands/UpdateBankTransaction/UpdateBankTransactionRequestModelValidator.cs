using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.UpdateBankTransaction
{
    public class UpdateBankTransactionRequestModelValidator : AbstractValidator<UpdateBankTransactionRequestModel>
    {
        public UpdateBankTransactionRequestModelValidator()
        {
            RuleFor(p => p.Id).Required();
            RuleFor(p => p.BankAccountId).Required();
            RuleFor(p => p.CompanyId).Required();
            RuleFor(p => p.TransactionAmount).Required();
            RuleFor(p => p.TransactionType).IsInEnum();
            RuleFor(p => p.PaymentType).IsInEnum();
            RuleFor(p => p.TransactionDate).Required();
            RuleFor(p => p.ChequeNumber).Required();
        }
    }
}