using System;
using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class UpdateGatePassRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        /// <summary>
        /// The Type of the GatePass 1 = GetOut and 2 = GetIn
        /// </summary>
        public GatePassType Type { get; set; }

        /// <summary>
        /// The Direction of the GatePass 1. Milling/2. Stockpile/3. Outside
        /// </summary>
        public Direction Direction { get; set; }
        public int CompanyId { get; set; }
        public CompanyRequestModel Company { get; set; }
        public int VehicleId { get; set; }
        public VehicleRequestModel Vehicle { get; set; }
        public string BiltyNumber { get; set; }
        /// <summary>
        /// Quantity of Bags
        /// </summary>
        public double BagQuantity { get; set; }
        /// <summary>
        /// Total Expected Weight of Bags = Quantity of Bags x Expected Bag Weight
        /// </summary>
        public double BagWeight { get; set; }
        /// <summary>
        /// The Weight Measured by the Guys by Machine
        /// </summary>
        public double KandaWeight { get; set; }
        /// <summary>
        /// Total Maund = Total Actual Weight of Bags / 40
        /// </summary>
        public double TotalMaund { get; set; }
        /// <summary>
        /// ProductId of Product
        /// </summary>
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public DateTime CheckDateTime { get; set; }
    }

    public class UpdateGatePassRequestModelValidator : AbstractValidator<UpdateGatePassRequestModel>
    {
        public UpdateGatePassRequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Direction).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.CompanyId).Must(p => p > 0).When(p => p.Company == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.ProductId).Must(p => p > 0).When(p => p.Product == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.VehicleId).Must(p => p > 0).When(p => p.Vehicle == null).WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.BagWeight).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.TotalMaund).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.KandaWeight).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.BagQuantity).GreaterThan(0).WithMessage(Messages.LessThan(1));
            RuleFor(p => p.BiltyNumber).Must(p => !string.IsNullOrEmpty(p)).When(p => p.Type == GatePassType.GateOut)
                .WithMessage(Messages.EmptyError);
            RuleFor(p => p.CheckDateTime).GreaterThan(DateTime.MinValue).WithMessage(Messages.LessThan(DateTime.MinValue));
        }
    }
}