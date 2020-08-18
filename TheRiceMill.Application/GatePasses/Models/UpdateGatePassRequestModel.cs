using System;
using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class UpdateGatePassRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public CompanyType CompanyId { get; set; }
        /// <summary>
        /// The Type of the GatePass 1 = OutwardGatePass and 2 = InwardGatePass
        /// </summary>
        public GatePassType Type { get; set; }

        /// <summary>
        /// The Direction of the GatePass 1. Milling/2. Stockpile/3. Outside
        /// </summary>
        public int PartyId { get; set; }
        public PartyRequestModel Party { get; set; }
        public int VehicleId { get; set; }
        public VehicleRequestModel Vehicle { get; set; }
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }
        public double WeightPerBag { get; set; }
        /// <summary>
        /// The Weight Measured by the Guys by Machine
        /// </summary>
        public double KandaWeight { get; set; }
        public double EmptyWeight { get; set; }
        public double NetWeight { get; set; }
        /// <summary>
        /// Total Maund = Total Actual Weight of Bags / 40
        /// </summary>
        public double Maund { get; set; }
        /// <summary>
        /// ProductId of Product
        /// </summary>
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public DateTime DateTime { get; set; }
        public string Broker { get; set; }
        public string BiltyNumber { get; set; }
        public string LotNumber { get; set; }
    }

    public class UpdateGatePassRequestModelValidator : AbstractValidator<UpdateGatePassRequestModel>
    {
        public UpdateGatePassRequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.PartyId).Must(p => p > 0).When(p => p.Party == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.ProductId).Must(p => p > 0).When(p => p.Product == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.VehicleId).Must(p => p > 0).When(p => p.Vehicle == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.WeightPerBag).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.Maund).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.KandaWeight).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.EmptyWeight).GreaterThanOrEqualTo(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.NetWeight).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.BagQuantity).Required().GreaterThanOrEqualTo(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.BoriQuantity).Required().GreaterThanOrEqualTo(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.DateTime).GreaterThan(DateTime.MinValue).WithMessage(Messages.LessThan(DateTime.MinValue));
        }
    }
}