using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Vehicles.Models
{

    public class CreateVehicleRequestModel : IRequest<ResponseViewModel>
    {
        public string PlateNo { get; set; }

    }

    public class CreateVehicleRequestModelValidator : AbstractValidator<CreateVehicleRequestModel>
    {
        public CreateVehicleRequestModelValidator()
        {
            RuleFor(p => p.PlateNo).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
        }
    }
}