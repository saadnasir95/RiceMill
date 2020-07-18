using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Products.Models
{
    public class DeleteProductRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }

    public class DeleteProductRequestModelValidator : AbstractValidator<DeleteProductRequestModel>
    {
        public DeleteProductRequestModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}