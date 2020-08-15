using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    public class UpdateGatePassRequestHandler : IRequestHandler<UpdateGatePassRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateGatePassRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateGatePassRequestModel request, CancellationToken cancellationToken)
        {
            var gatePass = _context.GatePasses.GetBy(p => p.Id == request.Id);
            if (gatePass == null)
            {
                throw new NotFoundException(nameof(GatePass), request.Id);
            }
            request.Copy(gatePass);
            gatePass.Type = request.Type.ToInt();
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
            }
            _context.GatePasses.Update(gatePass);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new GatePassResponseModel()
            {
                Type = (GatePassType)request.Type,
                BagQuantity = request.BagQuantity,
                BoriQuantity = request.BoriQuantity,
                WeightPerBag = request.WeightPerBag,
                NetWeight = request.NetWeight,
                EmptyWeight = request.EmptyWeight,
                KandaWeight = request.KandaWeight,
                Maund = request.Maund,
                Broker = request.Broker,
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