using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Bank.Commands.CreateBank
{
    public class CreateBankRequestModelValidator : AbstractValidator<CreateBankRequestModel>
    {
        public CreateBankRequestModelValidator()
        {
            RuleFor(p => p.Name).Required();
        }
    }
}