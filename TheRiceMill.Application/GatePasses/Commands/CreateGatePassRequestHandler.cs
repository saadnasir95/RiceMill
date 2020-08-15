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
                LotNumber = request.LotNumber,
                BiltyNumber = request.BiltyNumber
            };
            Party party;
            Vehicle vehicle;
            Product product;
            if (!string.IsNullOrEmpty(request.Party?.Name))
            {
                party = _context.Parties.GetBy(p => p.NormalizedName.Equals(request.Party.Name.ToUpper()));
                if (party == null)
                {
                    gatePass.Party = new Party()
                    {
                        Name = request.Party.Name,
                        NormalizedName = request.Party.Name.ToUpper(),
                        PhoneNumber = request.Party.PhoneNumber,
                        Address = request.Party.Address
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
                party = _context.Parties.GetBy(p => p.Id == request.PartyId);
                if (party == null)
                {
                    throw new NotFoundException(nameof(Party), request.PartyId);
                }
            }
            if (!string.IsNullOrEmpty(request.Vehicle?.PlateNo))
            {
                vehicle = _context.Vehicles.GetBy(p => p.PlateNo.Equals(request.Vehicle.PlateNo.ToUpper()));
                if (vehicle == null)
                {
                    gatePass.Vehicle = new Vehicle()
                    {
                        PlateNo = request.Vehicle.PlateNo.ToUpper(),
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
                vehicle = _context.Vehicles.GetBy(p => p.Id == request.VehicleId);
                if (vehicle == null)
                {
                    throw new NotFoundException(nameof(Vehicle), request.VehicleId);
                }
            }

            if (!string.IsNullOrEmpty(request.Product?.Name))
            {
                product = _context.Products.GetBy(p => p.Name.Equals(request.Product.Name.ToUpper()));
                if (product == null)
                {
                    gatePass.Product = new Product()
                    {
                        Name = request.Product.Name,
                        NormalizedName = request.Product.Name.ToUpper()
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
                product = _context.Products.GetBy(p => p.Id == request.ProductId);
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
                    PhoneNumber = party.PhoneNumber
                },
                Product = new ProductRequestModel()
                {
                    Name = product.Name
                },
                Vehicle = new VehicleRequestModel()
                {
                    PlateNo = vehicle.PlateNo
                },
                PartyId = party.Id,
                ProductId = product.Id,
                VehicleId = vehicle.Id,
                BiltyNumber = request.BiltyNumber,
                LotNumber = request.LotNumber
            });
        }
    }
}