using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Application.Ledgers.Queries.GetLedgers;

namespace TheRiceMill.Application.Ledgers.Queries.GetCompanyLedger
{
    public class GetCompanyLedgerRequestModelValidator : AbstractValidator<GetCompanyLedgerRequestModel>
    {
        public GetCompanyLedgerRequestModelValidator()
        {
            RuleFor(p => p.Page).Required();
            RuleFor(p => p.PageSize).Required();
            RuleFor(p => p.LedgerType).Required();
        }
    }
}
