using System;
using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class CheckOutGatePassRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public DateTime CheckOutTime { get; set; }
    }

    public class CheckOutGatePassRequestViewModelValidator : AbstractValidator<CheckOutGatePassRequestModel>
    {
        public CheckOutGatePassRequestViewModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.CheckOutTime).GreaterThan(DateTime.MinValue)
                .WithMessage(Messages.LessThan(DateTime.MinValue));
        }
    }
}