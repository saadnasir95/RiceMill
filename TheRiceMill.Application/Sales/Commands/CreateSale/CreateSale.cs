using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sales.Shared;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sales.Commands.CreateSale
{

    public class CreateSaleRequestHandler : IRequestHandler<CreateSaleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;
        GatepassMapper gatepassMapper = new GatepassMapper();

        public CreateSaleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateSaleRequestModel request, CancellationToken cancellationToken)
        {
            var sale = new Domain.Entities.Sale();
            request.Copy(sale);
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

            sale.Charges = new List<Charge>();
            sale.Charges = charges;
            sale.RateBasedOn = request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bag;
            sale.Commission = request.Commission;
            sale.Rate = request.Rate;
            sale.TotalPrice = request.TotalPrice;
            sale.Freight = request.Freight;
            sale.BasePrice = request.BasePrice;
            sale.TotalMaund = request.TotalMaund;
            sale.BoriQuantity = request.BoriQuantity;
            sale.BagQuantity = request.BagQuantity;
            sale.Date = request.Date;
            sale.CompanyId = request.CompanyId.ToInt();
            _context.Sales.Add(sale);

            List<GatePass> gatepasses = new List<GatePass>();
            GatePass gatepass = null;
            foreach (var id in request.GatepassIds)
            {
                gatepass = _context.GatePasses.GetBy(q => q.Id == id, p => p.Include(pr => pr.Party).Include(pr => pr.Product).Include(pr => pr.Vehicle));
                gatepasses.Add(gatepass);
                gatepass.SaleId = sale.Id;

            }
            await _context.SaveChangesAsync(cancellationToken);
            var transactionId = Guid.NewGuid().ToString();
            var companyLedger = new Domain.Entities.Ledger()
            {
                PartyId = gatepass.PartyId,
                Amount = -request.TotalPrice,
                Id = sale.Id,
                TransactionType = TransactionType.Company.ToInt(),
                LedgerType = (int)LedgerType.Sale,
                TransactionId = transactionId,
                Date = sale.Date,
                CompanyId = request.CompanyId.ToInt()
            };
            var partyLedger = new Domain.Entities.Ledger()
            {
                PartyId = gatepass.PartyId,
                Amount = request.TotalPrice - request.Commission,
                Id = sale.Id,
                TransactionType = TransactionType.Party.ToInt(),
                LedgerType = (int)LedgerType.Sale,
                TransactionId = transactionId,
                Date = sale.Date,
                CompanyId = request.CompanyId.ToInt()
            };
            _context.Add(companyLedger);
            _context.Add(partyLedger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new SaleResponseViewModel()
            {
                TotalMaund = request.TotalMaund,
                BagQuantity = request.BagQuantity,
                BoriQuantity = request.BoriQuantity,
                Gatepasses = gatepassMapper.MapFull(gatepasses),
                Id = sale.Id,
                RateBasedOn = (int)sale.RateBasedOn,
                BasePrice = sale.BasePrice,
                Freight = sale.Freight,
                Commission = sale.Commission,
                AdditionalCharges = request.AdditionalCharges,
                TotalPrice = sale.TotalPrice,
                Rate = sale.Rate,
                Date = new DateConverter().ConvertToDateTimeIso(sale.Date),
                CreatedDate = new DateConverter().ConvertToDateTimeIso(sale.CreatedDate),
                CompanyId = (CompanyType)sale.CompanyId
            });
        }
    }
}