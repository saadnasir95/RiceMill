using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sale.Shared;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sale.Commands.UpdateSale
{

    public class UpdateSaleRequestHandler : IRequestHandler<UpdateSaleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateSaleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateSaleRequestModel request, CancellationToken cancellationToken)
        {
            var sale = _context.Sales.GetBy(p => p.Id == request.Id, p => p.Include(pr => pr.Charges));
            if (sale == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Sale),request.Id);
            }
            var ledger = _context.Ledgers.GetBy(p => p.TransactionId == request.Id && p.LedgerType == (int)LedgerType.Sale);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger),request.Id);
            }
            request.Copy(sale);
            Party party;
            Vehicle vehicle;
            Product product;
            if (!string.IsNullOrEmpty(request.Party?.Name))
            {
                party = _context.Parties.GetBy(p => p.NormalizedName.Equals(request.Party.Name.ToUpper()));
                if (party == null)
                {
                    sale.Party = new Party()
                    {
                        Name = request.Party.Name,
                        NormalizedName = request.Party.Name.ToUpper(),
                        PhoneNumber = request.Party.PhoneNumber,
                        Address = request.Party.Address
                    };
                    party = sale.Party;
                }
                else
                {
                    sale.PartyId = party.Id;
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
                    sale.Vehicle = new Vehicle()
                    {
                        Name = request.Vehicle.Name,
                        NormalizedName = request.Vehicle.Name.ToUpper(),
                        PlateNo = request.Vehicle.PlateNo.ToUpper(),
                    };
                    vehicle = sale.Vehicle;
                }
                else
                {
                    sale.VehicleId = vehicle.Id;
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
                    sale.Product = new Product()
                    {
                        Name = request.Product.Name,
                        NormalizedName = request.Product.Name.ToUpper(),
                        Type = request.Product.Type.ToInt(),
                        Price = request.Product.Price,
                    };
                    product = sale.Product;
                }
                else
                {
                    sale.ProductId = product.Id;
                }
            }
            else
            {
                product = _context.Products.GetBy(p => p.Id == request.ProductId);
            }

            if (request.AdditionalCharges != null)
            {
                if (sale.Charges != null && sale.Charges.Any())
                {
                    _context.Charges.RemoveRange(sale.Charges);
                    await _context.SaveChangesAsync(cancellationToken);
                    sale.Charges.Clear();
                }
                else
                    sale.Charges = new List<Charge>();
                foreach (var charge in request.AdditionalCharges)
                {
                    sale.Charges.Add(new Charge()
                    {
                        BagQuantity = charge.BagQuantity,
                        Rate = charge.Rate,
                        Task = charge.Task,
                        Total = charge.Total,
                        AddPrice = charge.AddPrice,
                    });
                }
            }
            _context.Sales.Update(sale);
            ledger.PartyId = party.Id;
            ledger.Debit = request.TotalPrice;
            ledger.Credit = 0;
            _context.Ledgers.Update(ledger);

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new SaleResponseViewModel()
            {
                Vehicle = new VehicleRequestModel()
                {
                    Name   = vehicle.Name,
                    PlateNo = vehicle.PlateNo
                },
                Party = new PartyRequestModel()
                {
                    Name = party.Name,
                    Address = party.Address,
                    PhoneNumber = party.PhoneNumber,
                },
                Product = new ProductRequestModel()
                {
                    Name  = product.Name,
                    Price = product.Price,
                    Type = (ProductType)product.Type
                },
                PartyId = party.Id,
                VehicleId = vehicle.Id,
                ProductId = product.Id,
                BagQuantity = request.BagQuantity,
                BagWeight = request.BagWeight,
                KandaWeight = request.KandaWeight,
                TotalMaund = request.TotalMaund,
                CheckOut = new DateConverter().ConvertToDateTimeIso(request.CheckOut),
                Id = sale.Id,
                Commission = sale.Commission,
                AdditionalCharges = request.AdditionalCharges,
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

            });
        }
    }

}