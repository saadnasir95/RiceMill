using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Bank.Queries.GetBanks
{
    public class GetBanksRequestModelValidator : AbstractValidator<GetBanksRequestModel>
    {
        public GetBanksRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
        }
    }
}