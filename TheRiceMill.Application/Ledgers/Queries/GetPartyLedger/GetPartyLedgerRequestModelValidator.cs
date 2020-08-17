using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgers
{
    public class GetPartyLedgerRequestModelValidator : AbstractValidator<GetPartyLedgerRequestModel>
    {
        public GetPartyLedgerRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
            RuleFor(p => p.PartyId).Required();
        }
    }
}