using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.CreateBankTransaction
{
    public class CreateBankTransactionRequestModelValidator : AbstractValidator<CreateBankTransactionRequestModel>
    {
        public CreateBankTransactionRequestModelValidator()
        {
            RuleFor(p => p.PartyId).Required();
            RuleFor(p => p.TransactionAmount).Required();
            RuleFor(p => p.BankAccountId).Required();
            RuleFor(p => p.TransactionType).IsInEnum();
            RuleFor(p => p.PaymentType).IsInEnum();
            RuleFor(p => p.TransactionDate).Required();
            RuleFor(p => p.ChequeNumber).Required();
        }
    }
}