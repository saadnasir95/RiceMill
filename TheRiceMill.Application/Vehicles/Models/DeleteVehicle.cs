using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Vehicles.Models
{

    public class DeleteVehicleRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }

    public class DeleteVehicleRequestModelValidator : AbstractValidator<DeleteVehicleRequestModel>
    {
        public DeleteVehicleRequestModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}