using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Heads.Models
{
    public class CreateHead2RequestModel : IRequest<ResponseViewModel>
    {
        public int Head1Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
    }
    public class CreateHead2RequestModelValidator : AbstractValidator<CreateHead2RequestModel>
    {
        public CreateHead2RequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
            RuleFor(p => p.Head1Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}
