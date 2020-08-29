using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sales.Shared;
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
        GatepassMapper gatepassMapper = new GatepassMapper();

        public CreatePurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreatePurchaseRequestModel request, CancellationToken cancellationToken)
        {
            var purchase = new Purchase();
            request.Copy(purchase);
            List<Charge> charges = new List<Charge>();
            if (request.AdditionalCharges != null && request.AdditionalCharges.Any())
            {
                foreach (var charge in request.AdditionalCharges)
                {
                    charges.Add(new Charge()
                    {
                        BagQuantity = charge.BagQuantity,
                        Rate = charge.Rate,
                        Task = charge.Task,
                        Total = charge.Total,
                        AddPrice = charge.AddPrice,

                    });
                }
            }

            purchase.Charges = new List<Charge>();
            purchase.Charges = charges;
            purchase.RateBasedOn = request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bag;
            purchase.Commission = request.Commission;
            purchase.Rate = request.Rate;
            purchase.Freight = request.Freight;
            purchase.BasePrice = request.BasePrice;
            purchase.TotalPrice = request.TotalPrice;
            purchase.TotalMaund = request.TotalMaund;
            purchase.BoriQuantity = request.BoriQuantity;
            purchase.BagQuantity = request.BagQuantity;
            purchase.Date = request.Date;
            purchase.CompanyId = request.CompanyId.ToInt();
            _context.Purchases.Add(purchase);

            List<GatePass> gatepasses = new List<GatePass>();
            GatePass gatepass = null;
            foreach (var id in request.GatepassIds)
            {
                gatepass = _context.GatePasses.GetBy(q => q.Id == id, p => p.Include(pr => pr.Party).Include(pr => pr.Product).Include(pr => pr.Vehicle));
                gatepasses.Add(gatepass);
                gatepass.PurchaseId = purchase.Id;

            }
            await _context.SaveChangesAsync(cancellationToken);
            var transactionId = Guid.NewGuid().ToString();
            var companyLedger = new Domain.Entities.Ledger()
            {
                PartyId = gatepass.PartyId,
                Amount = -request.TotalPrice,
                Id = purchase.Id,
                TransactionType = TransactionType.Company.ToInt(),
                LedgerType = (int)LedgerType.Purchase,
                TransactionId = transactionId,
                Date = purchase.Date,
                CompanyId = request.CompanyId.ToInt()
            };
            var partyLedger = new Domain.Entities.Ledger()
            {
                PartyId = gatepass.PartyId,
                Amount = request.TotalPrice - request.Commission,
                Id = purchase.Id,
                TransactionType = TransactionType.Party.ToInt(),
                LedgerType = (int)LedgerType.Purchase,
                TransactionId = transactionId,
                Date = purchase.Date,
                CompanyId = request.CompanyId.ToInt()
            };
            _context.Add(companyLedger);
            _context.Add(partyLedger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new PurchaseResponseViewModel()
            {
                TotalMaund = request.TotalMaund,
                BagQuantity = request.BagQuantity,
                BoriQuantity = request.BoriQuantity,
                Gatepasses = gatepassMapper.MapFull(gatepasses),
                Id = purchase.Id,
                BasePrice = purchase.BasePrice,
                Freight = purchase.Freight,
                RateBasedOn = (int)purchase.RateBasedOn,
                Commission = purchase.Commission,
                AdditionalCharges = request.AdditionalCharges,
                TotalPrice = purchase.TotalPrice,
                Rate = purchase.Rate,
                Date = new DateConverter().ConvertToDateTimeIso(purchase.Date),
                CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate),
                CompanyId = (CompanyType)purchase.CompanyId
            }); ;
        }
    }

}