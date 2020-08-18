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
using TheRiceMill.Application.Sales.Commands.CreateSale;
using TheRiceMill.Common.Extensions;
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
                query = p => p.Id.ToString() == request.Search && p.CompanyId == request.CompanyId.ToInt();
            }
            else
            {
                query = p => p.Id != 0 && p.CompanyId == request.CompanyId.ToInt();
            }
            var purchase = _context.Purchases.GetMany(
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
                        EmptyWeight = gp.EmptyWeight,
                        NetWeight = gp.NetWeight,
                        Maund = gp.Maund,
                        DateTime = gp.DateTime,
                        Broker = gp.Broker,
                        KandaWeight = gp.KandaWeight,
                        LotNumber = gp.LotNumber,
                        Id = gp.Id,
                        Party = new PartyRequestModel()
                        {
                            Address = gp.Party.Address,
                            Name = gp.Party.Name,
                            PhoneNumber = gp.Party.PhoneNumber,
                            CompanyId = (CompanyType)gp.Party.CompanyId
                        },
                        Product = new ProductRequestModel()
                        {
                            Name = gp.Product.Name,
                            CompanyId = (CompanyType)gp.Product.CompanyId
                        },
                        Vehicle = new VehicleRequestModel()
                        {
                            PlateNo = gp.Vehicle.PlateNo,
                            CompanyId = (CompanyType)gp.Vehicle.CompanyId
                        },
                        PartyId = gp.PartyId,
                        ProductId = gp.ProductId,
                        VehicleId = gp.VehicleId,
                        PurchaseId = gp.PurchaseId,
                        SaleId = gp.SaleId,
                        CompanyId = (CompanyType)gp.CompanyId,
                        BiltyNumber = gp.BiltyNumber
                    }).ToList(),
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(p.Date),
                    Commission = p.Commission,
                    Id = p.Id,
                    CompanyId = (CompanyType)p.CompanyId,
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
                p.Id.ToString().Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt());
            return Task.FromResult(new ResponseViewModel().CreateOk(purchase, count));
        }
    }
}