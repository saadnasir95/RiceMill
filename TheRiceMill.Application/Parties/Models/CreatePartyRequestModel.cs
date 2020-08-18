using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class CreatePartyRequestModel : IRequest<ResponseViewModel>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public CompanyType CompanyId { get; set; }
    }
    public class CreatePartyRequestModelValidator : AbstractValidator<CreatePartyRequestModel>
    {
        public CreatePartyRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Name).NotEmpty().WithMessage(Messages.EmptyError).MaximumLength(50).WithMessage(Messages.MaxLengthError(50));
        }
    }

}