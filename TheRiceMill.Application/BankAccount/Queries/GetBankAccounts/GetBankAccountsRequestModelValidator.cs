using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.BankAccount.Queries.GetBankAccounts
{
    public class GetBankAccountsRequestModelValidator : AbstractValidator<GetBankAccountsRequestModel>
    {
        public GetBankAccountsRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
        }
    }
}