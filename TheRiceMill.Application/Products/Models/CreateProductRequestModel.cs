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
        public double Price { get; set; }
        public ProductType Type { get; set; }
    }

    public class CreateProductRequestModelValidator : AbstractValidator<CreateProductRequestModel>
    {
        public CreateProductRequestModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
            RuleFor(p => p.Price).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.Type).IsInEnum().WithMessage(Messages.IncorrectValue);
        }
    }
}