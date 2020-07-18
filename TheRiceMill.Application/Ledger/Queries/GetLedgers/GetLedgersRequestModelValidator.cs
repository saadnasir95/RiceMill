using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgers
{
    public class GetLedgersRequestModelValidator : AbstractValidator<GetLedgersRequestModel>
    {
        public GetLedgersRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
            RuleFor(p => p.CompanyId).Required();
        }
    }
}