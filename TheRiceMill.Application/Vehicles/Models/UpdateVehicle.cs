﻿using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Vehicles.Models
{

    public class UpdateVehicleRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public CompanyType CompanyId { get; set; }

    }

    public class UpdateVehicleRequestModelValidator : AbstractValidator<UpdateVehicleRequestModel>
    {
        public UpdateVehicleRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.PlateNo).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));

        }
    }
}