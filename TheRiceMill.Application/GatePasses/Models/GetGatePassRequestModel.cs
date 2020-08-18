﻿using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{

    public class GetGatePassRequestModel : IRequest<ResponseViewModel>
    {
        public void SetDefaultValue()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = "";
            }

            if (string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderBy = "CreatedDate";
            }

        }
        public CompanyType CompanyId { get; set; }
        public string Search { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool InvoicePendingGatePass { get; set; }
        public GatePassType GatePassType { get; set; }
        public int PartyId { get; set; }
    }

    public class GetGatePassRequestModelValidator : AbstractValidator<GetGatePassRequestModel>
    {
        public GetGatePassRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Page).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.PageSize).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}