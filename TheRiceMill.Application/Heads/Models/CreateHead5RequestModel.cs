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
    public class CreateHead5RequestModel : IRequest<ResponseViewModel>
    {
        public int Head4Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
    }
    public class CreateHead5RequestModelValidator : AbstractValidator<CreateHead5RequestModel>
    {
        public CreateHead5RequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
            RuleFor(p => p.Head4Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}

