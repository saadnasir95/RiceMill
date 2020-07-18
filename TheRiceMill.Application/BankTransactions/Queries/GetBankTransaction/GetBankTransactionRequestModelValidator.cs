using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankTransactions.Queries.GetBankTransaction
{
    public class GetBankTransactionRequestModelValidator : AbstractValidator<GetBankTransactionRequestModel>
    {
        public GetBankTransactionRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
        }
    }
}