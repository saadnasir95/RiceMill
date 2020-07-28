using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.GatePasses.Queries
{

    public class GetGatePassRequestHandler : IRequestHandler<GetGatePassRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetGatePassRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetGatePassRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var converter = new DateConverter();
            Expression<Func<GatePass, bool>> query = null;
            if (request.InvoicePendingGatePass)
            {
                query = p => (p.PurchaseId == null && p.SaleId == null) && (p.Party.Name.Contains(request.Search) ||
                                    (p.Id + "" == request.Search) ||
                                    p.Vehicle.PlateNo.Contains(request.Search) ||
                                    p.Product.Name.Contains(request.Search));
            }
            else
            {
                query = p => (p.Party.Name.Contains(request.Search) ||
                                    (p.Id + "" == request.Search) ||
                                    p.Vehicle.PlateNo.Contains(request.Search) ||
                                    p.Product.Name.Contains(request.Search));
            }

            List<GatePassResponseModel> gatePasses = _context.GatePasses
                .GetMany(query,
                request.OrderBy, request.Page,
                request.PageSize, request.IsDescending,
                p => p.Include(pr => pr.Party).Include(pr => pr.Vehicle).Include(pr => pr.Product))
                .Select(p => new GatePassResponseModel()
                {
                    Type = (GatePassType)p.Type,
                    BagQuantity = p.BagQuantity,
                    BoriQuantity = p.BoriQuantity,
                    WeightPerBag = p.WeightPerBag,
                    NetWeight = p.NetWeight,
                    Maund = p.Maund,
                    DateTime = p.DateTime,
                    Broker = p.Broker,
                    Id = p.Id,
                    Party = new PartyRequestModel()
                    {
                        Address = p.Party.Address,
                        Name = p.Party.Name,
                        PhoneNumber = p.Party.PhoneNumber
                    },
                    Product = new ProductRequestModel()
                    {
                        Name = p.Product.Name,
                        Price = p.Product.Price,
                        Type = (ProductType)p.Product.Type,
                    },
                    Vehicle = new VehicleRequestModel()
                    {
                        PlateNo = p.Vehicle.PlateNo,
                        Name = p.Vehicle.Name,
                    },
                    PartyId = p.PartyId,
                    ProductId = p.ProductId,
                    VehicleId = p.VehicleId,
                    PurchaseId = p.PurchaseId,
                    SaleId = p.SaleId
                }).ToList();
            var count = await _context.GatePasses.CountAsync(query, cancellationToken);
            return new ResponseViewModel().CreateOk(gatePasses, count);
        }
    }

}