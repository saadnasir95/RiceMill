using FluentValidation;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class GetPartyRequestModel : IRequest<ResponseViewModel>
    {
        public void SetDefaultValue()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = "";
            }

            if (string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderBy = "CreatedDate";
            }
        }
        public string Search { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public CompanyType CompanyId { get; set; }
    }

    public class GetPartyRequestModelValidator : AbstractValidator<GetPartyRequestModel>
    {
        public GetPartyRequestModelValidator()
        {
            RuleFor(p => p.CompanyId).IsInEnum().WithMessage(Messages.IncorrectValue);
            RuleFor(p => p.Page).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.PageSize).GreaterThan(0).WithMessage(Messages.LessThan(0));
        }
    }
}