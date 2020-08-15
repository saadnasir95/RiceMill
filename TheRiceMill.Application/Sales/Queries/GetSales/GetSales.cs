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
using TheRiceMill.Application.Sales.Commands.CreateSale;
using TheRiceMill.Application.Sales.Shared;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sales.Queries.GetSales
{
    public class GetSalesRequestHandler : IRequestHandler<GetSalesRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;
        GatepassMapper gatepassMapper = new GatepassMapper();

        public GetSalesRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetSalesRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Sale, bool>> query = null;
            if (request.Search.Length > 0)
            {
                query = p => p.Id.ToString() == request.Search;
            }
            else
            {
                query = p => p.Id != 0;
            }
            var sale = _context.Sales.GetMany(
                    query,
                    request.OrderBy, request.Page, request.PageSize, request.IsDescending,
                    p => p.Include(pr => pr.GatePasses))
                .Select(p => new
                {
                    p.Date,
                    Rate = p.Rate,
                    TotalPrice = p.TotalPrice,
                    BagQuantity = p.BagQuantity,
                    TotalMaund = p.TotalMaund,
                    BoriQuantity = p.BoriQuantity,
                    RateBasedOn = (int)p.RateBasedOn,
                    Gatepasses = p.GatePasses.Select(gp => new GatePassResponseModel()
                    {
                        Type = (GatePassType)gp.Type,
                        BagQuantity = gp.BagQuantity,
                        BoriQuantity = gp.BoriQuantity,
                        WeightPerBag = gp.WeightPerBag,
                        NetWeight = gp.NetWeight,
                        Maund = gp.Maund,
                        KandaWeight = gp.KandaWeight,
                        LotNumber = gp.LotNumber,
                        DateTime = gp.DateTime,
                        Broker = gp.Broker,
                        Id = gp.Id,
                        Party = new PartyRequestModel()
                        {
                            Address = gp.Party.Address,
                            Name = gp.Party.Name,
                            PhoneNumber = gp.Party.PhoneNumber
                        },
                        Product = new ProductRequestModel()
                        {
                            Name = gp.Product.Name
                        },
                        Vehicle = new VehicleRequestModel()
                        {
                            PlateNo = gp.Vehicle.PlateNo
                        },
                        PartyId = gp.PartyId,
                        ProductId = gp.ProductId,
                        VehicleId = gp.VehicleId,
                        PurchaseId = gp.PurchaseId,
                        SaleId = gp.SaleId
                    }).ToList(),
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
            return await Task.FromResult(new ResponseViewModel().CreateOk(sale, count));
        }
    }
}