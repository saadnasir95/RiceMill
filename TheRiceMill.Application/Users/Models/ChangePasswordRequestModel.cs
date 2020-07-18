using FluentValidation;
using MediatR;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Users.Models
{
    public class ChangePasswordRequestModel : IRequest<ResponseViewModel>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
    }

    public class ChangePasswordRequestModelValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordRequestModelValidator()
        {
            RuleFor(p => p.CurrentPassword).Password(8);
            RuleFor(p => p.NewPassword).Password(8);
            RuleFor(p => p.ConfirmPassword).Password(8).Equal(p => p.NewPassword).WithMessage("must match NewPassword");
        }
    }
}