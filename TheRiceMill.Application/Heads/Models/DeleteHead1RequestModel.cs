using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Heads.Models
{
    public class DeleteHead1RequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }

    public class DeleteHead1RequestModelValidator : AbstractValidator<DeleteHead1RequestModel>
    {
        public DeleteHead1RequestModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}
