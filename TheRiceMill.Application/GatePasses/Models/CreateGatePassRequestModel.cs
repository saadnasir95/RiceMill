using System;
using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class CreateGatePassRequestModel : IRequest<ResponseViewModel>
    {
        /// <summary>
        /// The Type of the GatePass 1 = GetOut and 2 = GetIn
        /// </summary>
        public GatePassType Type { get; set; }
        public int PartyId { get; set; }
        public PartyRequestModel Party { get; set; }
        public int VehicleId { get; set; }
        public VehicleRequestModel Vehicle { get; set; }
        /// <summary>
        /// Quantity of Bags
        /// </summary>
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }
        /// <summary>
        /// Total Expected Weight of Bags = Quantity of Bags x Expected Bag Weight
        /// </summary>
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
        public string Broker { get; set; }
        /// <summary>
        /// ProductId of Product
        /// </summary>
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public DateTime DateTime { get; set; }
        public string BiltyNumber { get; set; }
        public string LotNumber { get; set; }
    }

    public class VehicleRequestModel
    {
        public string Name { get; set; }
        public string PlateNo { get; set; }
    }
    public class PartyRequestModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class BankRequestModel
    {
        
    }
    public class ProductRequestModel
    {
        /// <summary>
        /// Navigation Property for Product
        /// </summary>
        public string Name { get; set; }
    }
    public class CreateGatePassRequestModelValidator : AbstractValidator<CreateGatePassRequestModel>
    {
        public CreateGatePassRequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
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