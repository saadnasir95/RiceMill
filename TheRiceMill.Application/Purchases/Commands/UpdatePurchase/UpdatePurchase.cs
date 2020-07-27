using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sale.Shared;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Purchases.Commands.UpdatePurchase
{

    public class UpdatePurchaseRequestHandler : IRequestHandler<UpdatePurchaseRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdatePurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdatePurchaseRequestModel request, CancellationToken cancellationToken)
        {
            var purchase = _context.Purchases.GetBy(p => p.Id == request.Id, p => p.Include(pr => pr.Charges));
            if (purchase == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Sale), request.Id);
            }
            var ledger = _context.Ledgers.GetBy(p => p.TransactionId == request.Id && p.LedgerType == (int)LedgerType.Purchase);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger), request.Id);
            }
            request.Copy(purchase);
            purchase.Direction = request.Direction.ToInt();
            Company company;
            Vehicle vehicle;
            Product product;
            if (!string.IsNullOrEmpty(request.Company?.Name))
            {
                company = _context.Companies.GetBy(p => p.NormalizedName.Equals(request.Company.Name.ToUpper()));
                if (company == null)
                {
                    purchase.Company = new Company()
                    {
                        Name = request.Company.Name,
                        NormalizedName = request.Company.Name.ToUpper(),
                        PhoneNumber = request.Company.PhoneNumber,
                        Address = request.Company.Address
                    };
                    company = purchase.Company;
                }
                else
                {
                    purchase.CompanyId = company.Id;
                }
            }
            else
            {
                company = _context.Companies.GetBy(p => p.Id == request.CompanyId);
            }
            if (!string.IsNullOrEmpty(request.Vehicle?.PlateNo))
            {
                vehicle = _context.Vehicles.GetBy(p => p.PlateNo.Equals(request.Vehicle.PlateNo.ToUpper()));
                if (vehicle == null)
                {
                    purchase.Vehicle = new Vehicle()
                    {
                        Name = request.Vehicle.Name,
                        NormalizedName = request.Vehicle.Name.ToUpper(),
                        PlateNo = request.Vehicle.PlateNo.ToUpper(),
                    };
                    vehicle = purchase.Vehicle;
                }
                else
                {
                    purchase.VehicleId = vehicle.Id;
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
                    purchase.Product = new Product()
                    {
                        Name = request.Product.Name,
                        NormalizedName = request.Product.Name.ToUpper(),
                        Type = request.Product.Type.ToInt(),
                        Price = request.Product.Price,
                    };
                    product = purchase.Product;
                }
                else
                {
                    purchase.ProductId = product.Id;
                }
            }
            else
            {
                product = _context.Products.GetBy(p => p.Id == request.ProductId);
            }

            if (request.AdditionalCharges != null)
            {
                if (purchase.Charges != null && purchase.Charges.Any())
                {
                    _context.Charges.RemoveRange(purchase.Charges);
                    await _context.SaveChangesAsync(cancellationToken);
                    purchase.Charges.Clear();
                }
                else
                    purchase.Charges = new List<Charge>();
                foreach (var charge in request.AdditionalCharges)
                {
                    purchase.Charges.Add(new Charge()
                    {
                        BagQuantity = charge.BagQuantity,
                        Rate = charge.Rate,
                        Task = charge.Task,
                        Total = charge.Total,
                        AddPrice = charge.AddPrice,
                    });
                }
            }
            _context.Purchases.Update(purchase);
            ledger.CompanyId = company.Id;
            ledger.Credit = request.TotalPrice;
            ledger.Debit = 0;
            _context.Ledgers.Update(ledger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new PurchaseResponseViewModel()
            {
                Vehicle = new VehicleRequestModel()
                {
                    Name = vehicle.Name,
                    PlateNo = vehicle.PlateNo
                },
                Product = new ProductRequestModel()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Type = (ProductType)product.Type
                },
                VehicleId = vehicle.Id,
                ProductId = product.Id,

                BagQuantity = request.BagQuantity,
                BagWeight = request.BagWeight,
                KandaWeight = request.KandaWeight,
                TotalMaund = request.TotalMaund,
                CheckIn = new DateConverter().ConvertToDateTimeIso(request.CheckIn),
                Id = purchase.Id,
                Commission = purchase.Commission,
                AdditionalCharges = request.AdditionalCharges,
                BasePrice = purchase.BasePrice,
                PercentCommission = purchase.PercentCommission,
                TotalPrice = purchase.TotalPrice,
                ActualBagWeight = purchase.ActualBagWeight,
                ExpectedBagWeight = purchase.ExpectedBagWeight,
                RatePerKg = purchase.RatePerKg,
                RatePerMaund = purchase.RatePerMaund,
                ExpectedEmptyBagWeight = purchase.ExpectedEmptyBagWeight,
                TotalActualBagWeight = purchase.TotalActualBagWeight,
                TotalExpectedBagWeight = purchase.TotalExpectedBagWeight,
                TotalExpectedEmptyBagWeight = purchase.TotalExpectedEmptyBagWeight,
                Direction = purchase.Direction,
                Vibration = purchase.Vibration,
                ActualBags = purchase.ActualBags,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate)
            });
        }
    }

}