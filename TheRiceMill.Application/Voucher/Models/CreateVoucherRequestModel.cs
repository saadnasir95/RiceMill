using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Voucher.Models
{
    public class CreateVoucherRequestModel : IRequest<ResponseViewModel>
    {
        public CompanyType CompanyId { get; set; }
        public DateTime DateTime { get; set; }
        public VoucherType Type { get; set; }
        public VoucherDetailType DetailType { get; set; }
        public List<VoucherDetail> VoucherDetails { get; set; }

    }

    public class VoucherDetail
    {
        public int PartyId { get; set; }
        public int SalesId { get; set; }
        public int PurchaseId { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Remarks { get; set; }
    }
    public class CreateVoucherRequestModelValidator : AbstractValidator<CreateVoucherRequestModel>
    {
        public CreateVoucherRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.DateTime).NotEmpty();
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.VoucherDetails).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.DetailType).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}
