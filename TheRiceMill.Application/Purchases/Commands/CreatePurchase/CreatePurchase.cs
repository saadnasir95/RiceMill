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
        GatepassMapper gatepassMapper = new GatepassMapper();

        public CreatePurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreatePurchaseRequestModel request, CancellationToken cancellationToken)
        {
            var purchase = new Purchase();
            request.Copy(purchase);
            List <Charge> charges = new List<Charge>();
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
            purchase.RateBasedOn =  request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bori;
            purchase.Commission = request.Commission;
            purchase.RatePerMaund = request.RatePerMaund;
            purchase.TotalPrice = request.TotalPrice;
            purchase.BoriQuantity = request.BoriQuantity;
            purchase.Date = request.Date;
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
            /*           var ledger = new Domain.Entities.Ledger()
                       {
                           //PartyId = party.Id,
                           Credit = request.TotalPrice,
                           Debit = 0,
                           Description = "",
                           TransactionId = purchase.Id,
                           LedgerType = (int)LedgerType.Purchase,
                       };*/
            //_context.Add(ledger);
            //await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new PurchaseResponseViewModel()
            {
                /*BagQuantity = request.BagQuantity,
                BagWeight = request.BagWeight,
                KandaWeight = request.KandaWeight,*/
                TotalMaund = request.TotalMaund,
                BoriQuantity = request.BoriQuantity,
                Gatepasses = gatepassMapper.MapFull(gatepasses),
                
                //CheckIn = new DateConverter().ConvertToDateTimeIso(request.CheckIn),
                Id = purchase.Id,
                RateBasedOn = purchase.RateBasedOn == RateBasedOn.Maund ? 1 : 2,
                Commission = purchase.Commission,
                AdditionalCharges = request.AdditionalCharges,
                /*BasePrice = purchase.BasePrice,
                 PercentCommission = purchase.PercentCommission,*/
                TotalPrice = purchase.TotalPrice,
                /*ActualBagWeight = purchase.ActualBagWeight,
                ExpectedBagWeight = purchase.ExpectedBagWeight,
                RatePerKg = purchase.RatePerKg,*/
                RatePerMaund = purchase.RatePerMaund,
                Date = new DateConverter().ConvertToDateTimeIso(purchase.Date),
                /*ExpectedEmptyBagWeight = purchase.ExpectedEmptyBagWeight,
                TotalActualBagWeight = purchase.TotalActualBagWeight,
                TotalExpectedBagWeight = purchase.TotalExpectedBagWeight,
                TotalExpectedEmptyBagWeight = purchase.TotalExpectedEmptyBagWeight,
                Direction = purchase.Direction,
                Vibration = purchase.Vibration,
                ActualBags = purchase.ActualBags*/
                CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate)
            }); ;
        }
    }

}