﻿using FluentValidation;
using MediatR;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Products.Models
{
    public class GetProductsRequestModel : IRequest<ResponseViewModel>
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
    }

    public class GetProductsRequestModelValidator : AbstractValidator<GetProductsRequestModel>
    {
        public GetProductsRequestModelValidator()
        {
            RuleFor(p => p.Page).GreaterThan(0).WithMessage(Messages.LessThan(0));
            RuleFor(p => p.PageSize).GreaterThan(0).WithMessage(Messages.LessThan(0));

        }
    }
}