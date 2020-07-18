using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class DeleteGatePassRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }

    public class DeleteGatePassRequestModelValidator : AbstractValidator<DeleteGatePassRequestModel>
    {
        public DeleteGatePassRequestModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}