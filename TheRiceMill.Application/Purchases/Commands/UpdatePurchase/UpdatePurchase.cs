using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sales.Shared;
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
            var partyledger = _context.Ledgers.GetBy(p => p.Id == request.Id && p.LedgerType == (int)LedgerType.Purchase && p.TransactionType == TransactionType.Party.ToInt());
            var companyLedger = _context.Ledgers.GetBy(p => p.Id == request.Id && p.LedgerType == (int)LedgerType.Purchase && p.TransactionType == TransactionType.Company.ToInt());
            if (partyledger == null || companyLedger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger), request.Id);
            }
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

                partyledger.Amount = request.TotalPrice - request.Commission;
                partyledger.CompanyId = request.CompanyId.ToInt();
                partyledger.Date = purchase.Date;
                companyLedger.Amount = -request.TotalPrice;
                companyLedger.CompanyId = request.CompanyId.ToInt();
                companyLedger.Date = purchase.Date;

                _context.Ledgers.Update(companyLedger);
                _context.Ledgers.Update(partyledger);

                purchase.RateBasedOn = request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bag;
                purchase.BoriQuantity = request.BoriQuantity;
                purchase.BagQuantity = request.BagQuantity;
                purchase.TotalMaund = request.TotalMaund;
                purchase.CompanyId = purchase.CompanyId.ToInt();
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(new PurchaseResponseViewModel()
                {

                    /*                    BagWeight = request.BagWeight,
                                        KandaWeight = request.KandaWeight,*/
                    TotalMaund = request.TotalMaund,
                    Id = purchase.Id,
                    Date = new DateConverter().ConvertToDateTimeIso(purchase.Date),
                    Commission = purchase.Commission,
                    AdditionalCharges = request.AdditionalCharges,
                    TotalPrice = purchase.TotalPrice,
                    Rate = purchase.Rate,
                    BagQuantity = purchase.BagQuantity,
                    BoriQuantity = purchase.BoriQuantity,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate),
                    CompanyId = (CompanyType)purchase.CompanyId
                });
            }
        }
    }
}