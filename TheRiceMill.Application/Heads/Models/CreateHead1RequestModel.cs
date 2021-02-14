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
    public class CreateHead1RequestModel : IRequest<ResponseViewModel>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public CompanyType CompanyId { get; set; }
    }
    public class CreateHead1RequestModelValidator : AbstractValidator<CreateHead1RequestModel>
    {
        public CreateHead1RequestModelValidator()
        {
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));

        }
    }
}
