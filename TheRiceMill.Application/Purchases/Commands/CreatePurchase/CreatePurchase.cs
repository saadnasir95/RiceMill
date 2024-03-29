using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sale.Shared;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Purchases.Commands.CreatePurchase
{

    public class CreatePurchaseRequestHandler : IRequestHandler<CreatePurchaseRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreatePurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreatePurchaseRequestModel request, CancellationToken cancellationToken)
        {
            var purchase = new Purchase();
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

            if (request.AdditionalCharges != null && request.AdditionalCharges.Any())
            {
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
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync(cancellationToken);
            var ledger = new Domain.Entities.Ledger()
            {
                CompanyId = company.Id,
                Credit = request.TotalPrice,
                Debit = 0,
                Description = "",
                TransactionId = purchase.Id,
                LedgerType = (int)LedgerType.Purchase,
            };
            _context.Add(ledger);
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
                Company = new CompanyRequestModel()
                {
                    Name = company.Name,
                    Address = company.Address,
                    PhoneNumber = company.PhoneNumber
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