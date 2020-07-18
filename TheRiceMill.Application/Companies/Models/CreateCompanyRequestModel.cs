using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class CreateCompanyRequestModel : IRequest<ResponseViewModel>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class CreateCompanyRequestModelValidator : AbstractValidator<CreateCompanyRequestModel>
    {
        public CreateCompanyRequestModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
        }
    }

}