using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Products.Models
{
    public class CreateProductRequestModel : IRequest<ResponseViewModel>
    {
        public string Name { get; set; }
        public CompanyType CompanyId { get; set; }
    }

    public class CreateProductRequestModelValidator : AbstractValidator<CreateProductRequestModel>
    {
        public CreateProductRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
        }
    }
}