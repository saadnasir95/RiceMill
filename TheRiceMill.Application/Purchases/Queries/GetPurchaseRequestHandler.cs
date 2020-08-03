using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Purchases.Queries
{
    public class GetPurchaseRequestHandler : IRequestHandler<GetPurchaseRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetPurchaseRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Purchase, bool>> query = null;
            if (request.Search.Length > 0)
            {
                query = p => p.Id.ToString() == request.Search;
            }else
            {
                query = p => p.Id != 0;
            }
            var purchase = _context.Purchases.GetMany(
                    query,
                    request.OrderBy, request.Page, request.PageSize, request.IsDescending)
                .Select(p => new
                {
                    RatePerMaund = p.RatePerMaund,
                    TotalPrice = p.TotalPrice,
                    TotalMaund = p.TotalMaund,
                    Gatepasses = p.GatePasses,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(p.Date),
                    Commission = p.Commission,
                    Id = p.Id,
                    AdditionalCharges = p.Charges.Select(pr => new ChargeRequestViewModel()
                    {
                        Id = pr.Id,
                        Rate = pr.Rate,
                        Task = pr.Task,
                        Total = pr.Total,
                        BagQuantity = pr.BagQuantity,
                        AddPrice = pr.AddPrice,
                    }).ToArray()
                }).ToList();
            var count = _context.Purchases.Count(p =>
                p.Id.ToString().Contains(request.Search));
            return Task.FromResult(new ResponseViewModel().CreateOk(purchase, count));
        }
    }
}