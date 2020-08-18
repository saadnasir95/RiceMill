using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgerInfo
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