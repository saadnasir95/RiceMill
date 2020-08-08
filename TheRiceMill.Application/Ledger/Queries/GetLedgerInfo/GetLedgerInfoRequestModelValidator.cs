using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgerInfo
{
    public class GetLedgerInfoRequestModelValidator : AbstractValidator<GetLedgerInfoRequestModel>
    {
        public GetLedgerInfoRequestModelValidator()
        {
            RuleFor(p => p.LedgerType).IsInEnum();
            RuleFor(p => p.Id).Required();
        }
    }
}