using System.Collections.Generic;
using System.Linq;
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
            /*var ledger = _context.Ledgers.GetBy(p => p.TransactionId == request.Id && p.LedgerType == (int)LedgerType.Purchase);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger), request.Id);
            }*/
            request.Copy(purchase);
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
            {

                var gatepasses = _context.GatePasses.Where(q => q.PurchaseId == purchase.Id).ToList();
                gatepasses.ForEach(gatepass =>
                {
                    var _gatepass = _context.GatePasses.Find(gatepass.Id);
                    _gatepass.PurchaseId = null;
                    _context.GatePasses.Update(_gatepass);
                });

                foreach (var id in request.GatepassIds)
                {
                    var gatepass = _context.GatePasses.GetBy(q => q.Id == id);
                    gatepass.PurchaseId = purchase.Id;
                    _context.GatePasses.Update(gatepass);
                }
                /*          ledger.PartyId = party.Id;
                            ledger.Credit = request.TotalPrice;
                            ledger.Debit = 0;
                            _context.Ledgers.Update(ledger);
                */
                purchase.RateBasedOn = request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bori;
                purchase.BoriQuantity = request.BoriQuantity;
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(new PurchaseResponseViewModel()
                {

                    /*              BagQuantity = request.BagQuantity,
                                    BagWeight = request.BagWeight,
                                    KandaWeight = request.KandaWeight,
                    */
                    /*                Vehicle = new VehicleRequestModel()
                                    {
                                        Name = gatepass.Vehicle.Name,
                                        PlateNo = gatepass.Vehicle.PlateNo
                                    },
                                    Product = new ProductRequestModel()
                                    {
                                        Name = gatepass.Product.Name,
                                        Price = gatepass.Product.Price,
                                        Type = (ProductType)gatepass.Product.Type
                                    },
                                    VehicleId = gatepass.Vehicle.Id,
                                    ProductId = gatepass.Product.Id,
                                    Party = new PartyRequestModel()
                                    {
                                        Name = gatepass.Party.Name,
                                        Address = gatepass.Party.Address,
                                        PhoneNumber = gatepass.Party.PhoneNumber
                                    },*/
                    TotalMaund = request.TotalMaund,
                    Id = purchase.Id,
                    Date = new DateConverter().ConvertToDateTimeIso(purchase.Date),
                    Commission = purchase.Commission,
                    AdditionalCharges = request.AdditionalCharges,
                    TotalPrice = purchase.TotalPrice,
                    RatePerMaund = purchase.RatePerMaund,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate)
                });
            }
        }

    }
}