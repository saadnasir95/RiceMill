using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;
using System.Linq;

namespace TheRiceMill.Application.GatePasses.Commands
{

    public class CreateGatePassRequestHandler : IRequestHandler<CreateGatePassRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateGatePassRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateGatePassRequestModel request,
            CancellationToken cancellationToken)
        {
            var gatePass = new GatePass()
            {
                Type = request.Type.ToInt(),
                PartyId = request.PartyId,
                CompanyId = request.CompanyId.ToInt(),
                VehicleId = request.VehicleId,
                DateTime = request.DateTime,
                ProductId = request.ProductId,
                BagQuantity = request.BagQuantity,
                BoriQuantity = request.BoriQuantity,
                WeightPerBag = request.WeightPerBag,
                KandaWeight = request.KandaWeight,
                EmptyWeight = request.EmptyWeight,
                NetWeight = request.NetWeight,
                Maund = request.Maund,
                Broker = request.Broker,
                BiltyNumber = request.BiltyNumber
            };

            Lot lot = _context.Lots.GetBy(c => c.Id == request.LotId && c.Year == request.LotYear);
            if (lot != null)
            {
                gatePass.LotId = lot.Id;
                gatePass.LotYear = lot.Year;
            }
            else
            {
                List<Lot> lots = _context.Lots.GetMany(c => c.Year == request.LotYear, "Id", 1, 1, true, null).ToList();
                lot = new Lot
                {
                    Id = (lots.FirstOrDefault()?.Id + 1) ?? 1,
                    CompanyId = request.CompanyId.ToInt(),
                    Year = request.LotYear,
                    StockIns = new List<StockIn>()
                };
                gatePass.Lot = lot;
            }
            if (request.Type == GatePassType.InwardGatePass)
            {
                lot.StockIns.Add(new StockIn
                {
                    BagQuantity = request.BagQuantity,
                    BoriQuantity = request.BoriQuantity,
                    TotalKG = request.NetWeight,
                    GatepassTime = request.DateTime
                });
            }
            Party party;
            Vehicle vehicle;
            Product product;
            if (!string.IsNullOrEmpty(request.Party?.Name))
            {
                party = _context.Parties.GetBy(p => p.NormalizedName.Equals(request.Party.Name.ToUpper()) && p.CompanyId == request.CompanyId.ToInt());
                if (party == null)
                {
                    gatePass.Party = new Party()
                    {
                        Name = request.Party.Name,
                        NormalizedName = request.Party.Name.ToUpper(),
                        PhoneNumber = request.Party.PhoneNumber,
                        Address = request.Party.Address,
                        CompanyId = request.CompanyId.ToInt()
                    };
                    party = gatePass.Party;
                }
                else
                {
                    gatePass.PartyId = party.Id;
                }
            }
            else
            {
                party = _context.Parties.GetBy(p => p.Id == request.PartyId && p.CompanyId == request.CompanyId.ToInt());
                if (party == null)
                {
                    throw new NotFoundException(nameof(Party), request.PartyId);
                }
            }
            if (!string.IsNullOrEmpty(request.Vehicle?.PlateNo))
            {
                vehicle = _context.Vehicles.GetBy(p => p.PlateNo.Equals(request.Vehicle.PlateNo.ToUpper()) && p.CompanyId == request.CompanyId.ToInt());
                if (vehicle == null)
                {
                    gatePass.Vehicle = new Vehicle()
                    {
                        PlateNo = request.Vehicle.PlateNo.ToUpper(),
                        CompanyId = request.CompanyId.ToInt()
                    };
                    vehicle = gatePass.Vehicle;
                }
                else
                {
                    gatePass.VehicleId = vehicle.Id;
                }
            }
            else
            {
                vehicle = _context.Vehicles.GetBy(p => p.Id == request.VehicleId && p.CompanyId == request.CompanyId.ToInt());
                if (vehicle == null)
                {
                    throw new NotFoundException(nameof(Vehicle), request.VehicleId);
                }
            }

            if (!string.IsNullOrEmpty(request.Product?.Name))
            {
                product = _context.Products.GetBy(p => p.Name.Equals(request.Product.Name.ToUpper()) && p.CompanyId == request.CompanyId.ToInt());
                if (product == null)
                {
                    gatePass.Product = new Product()
                    {
                        Name = request.Product.Name,
                        NormalizedName = request.Product.Name.ToUpper(),
                        CompanyId = request.CompanyId.ToInt()
                    };
                    product = gatePass.Product;
                }
                else
                {
                    gatePass.ProductId = product.Id;
                }
            }
            else
            {
                product = _context.Products.GetBy(p => p.Id == request.ProductId && p.CompanyId == request.CompanyId.ToInt());
                if (product == null)
                {
                    throw new NotFoundException(nameof(Product), request.ProductId);
                }
            }
            _context.GatePasses.Add(gatePass);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new GatePassResponseModel()
            {
                Type = request.Type,
                CompanyId = request.CompanyId,
                Broker = request.Broker,
                BagQuantity = request.BagQuantity,
                BoriQuantity = request.BoriQuantity,
                WeightPerBag = request.WeightPerBag,
                EmptyWeight = request.EmptyWeight,
                KandaWeight = request.KandaWeight,
                NetWeight = request.NetWeight,
                Maund = request.Maund,
                DateTime = request.DateTime,
                Id = gatePass.Id,
                Party = new PartyRequestModel()
                {
                    Address = party.Address,
                    Name = party.Name,
                    PhoneNumber = party.PhoneNumber,
                    CompanyId = request.CompanyId
                },
                Product = new ProductRequestModel()
                {
                    Name = product.Name,
                    CompanyId = request.CompanyId
                },
                Vehicle = new VehicleRequestModel()
                {
                    PlateNo = vehicle.PlateNo,
                    CompanyId = request.CompanyId
                },
                PartyId = party.Id,
                ProductId = product.Id,
                VehicleId = vehicle.Id,
                BiltyNumber = request.BiltyNumber,
                LotId = lot.Id,
                LotYear = request.LotYear
            });
        }
    }
}