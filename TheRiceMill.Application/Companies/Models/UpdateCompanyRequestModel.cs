using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class UpdateCompanyRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateCompanyRequestModelValidator : AbstractValidator<UpdateCompanyRequestModel>
    {
        public UpdateCompanyRequestModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
            RuleFor(p => p.Id).GreaterThan(0).WithMessage(Messages.LessThan(0));

        }
    }
}