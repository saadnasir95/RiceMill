using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Application.Sale.Shared;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sale.Queries.GetSales
{
    public class GetSalesRequestHandler : IRequestHandler<GetSalesRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetSalesRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetSalesRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Sale, bool>> searchQuery = p =>
                p.Party.Name.Contains(request.Search) || p.Product.Name.Contains(request.Search) ||
                p.Vehicle.PlateNo.Contains(request.Search);
            var list = _context.Sales.GetMany(searchQuery, request.OrderBy, request.Page,
                    request.PageSize, request.IsDescending,
                    p => p.Include(pr => pr.Party).Include(pr => pr.Product).Include(pr => pr.Vehicle))
                .Select(sale => new SaleResponseViewModel()
                {
                    Vehicle = new VehicleRequestModel()
                    {
                        PlateNo = sale.Vehicle.PlateNo
                    },
                    Party = new PartyRequestModel()
                    {
                        Name = sale.Party.Name,
                        Address = sale.Party.Address,
                        PhoneNumber = sale.Party.PhoneNumber,
                    },
                    Product = new ProductRequestModel()
                    {
                        Name = sale.Product.Name
                    },
                    PartyId = sale.Party.Id,
                    VehicleId = sale.Vehicle.Id,
                    ProductId = sale.Product.Id,
                    BagQuantity = sale.BagQuantity,
                    BagWeight = sale.BagWeight,
                    KandaWeight = sale.KandaWeight,
                    TotalMaund = sale.TotalMaund,
                    CheckOut = new DateConverter().ConvertToDateTimeIso(sale.CheckOut),
                    Id = sale.Id,
                    Commission = sale.Commission,
                    AdditionalCharges = sale.Charges.Select(p => new ChargeRequestViewModel()
                    {
                        Id = p.Id,
                        Rate = p.Rate,
                        Task = p.Task,
                        Total = p.Total,
                        BagQuantity = p.BagQuantity,
                        AddPrice = p.AddPrice,
                    }).ToArray(),
                    BasePrice = sale.BasePrice,
                    BiltyNumber = sale.BiltyNumber,
                    PercentCommission = sale.PercentCommission,
                    TotalPrice = sale.TotalPrice,
                    ActualBagWeight = sale.ActualBagWeight,
                    ExpectedBagWeight = sale.ExpectedBagWeight,
                    RatePerKg = sale.RatePerKg,
                    RatePerMaund = sale.RatePerMaund,
                    ExpectedEmptyBagWeight = sale.ExpectedEmptyBagWeight,
                    TotalActualBagWeight = sale.TotalActualBagWeight,
                    TotalExpectedBagWeight = sale.TotalExpectedBagWeight,
                    TotalExpectedEmptyBagWeight = sale.TotalExpectedEmptyBagWeight,
                }).ToList();
            var count = await _context.Sales.CountAsync(searchQuery, cancellationToken: cancellationToken);
            return (new ResponseViewModel().CreateOk(list, count));
        }
    }
}