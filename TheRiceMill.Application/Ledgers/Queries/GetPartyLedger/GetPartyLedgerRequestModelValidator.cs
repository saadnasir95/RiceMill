using FluentValidation;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgers
{
    public class GetPartyLedgerRequestModelValidator : AbstractValidator<GetPartyLedgerRequestModel>
    {
        public GetPartyLedgerRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
            RuleFor(p => p.PartyId).Required();
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}