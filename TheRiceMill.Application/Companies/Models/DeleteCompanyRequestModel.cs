using FluentValidation;
using MediatR;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class DeleteCompanyRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
    }

    public class DeleteCompanyRequestModelValidator : AbstractValidator<DeleteCompanyRequestModel>
    {
        public DeleteCompanyRequestModelValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}