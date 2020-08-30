using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;
using static TheRiceMill.Application.Ledgers.Queries.GetLedgerInfo.GetLedgerInfoRequestHandler;

namespace TheRiceMill.Application.Ledgers.Queries.GetLedgers
{
    public class GetPartyLedgerRequestHandler : IRequestHandler<GetPartyLedgerRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPartyLedgerRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///L1 C1000 D0 = 1000
        ///L2 C1000 D0 = 2000
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Handle(GetPartyLedgerRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Ledger, bool>> query = p => p.PartyId == request.PartyId && p.TransactionType == TransactionType.Party.ToInt() && p.CompanyId == request.CompanyId.ToInt();

            //PrevBalance = 10000

            var dateConverter = new DateConverter();
            var list = await _context.Ledgers
                .GetManyReadOnly(query, "Date", request.Page, request.PageSize, false,
                    p => p.Include(pr => pr.Party)).Select(p =>
                    new LedgerResponse()
                    {
                        Id = p.Id,
                        PartyId = p.PartyId,
                        LedgerType = p.LedgerType,
                        Amount = p.Amount,
                        Party = p.Party,
                        TransactionType = p.TransactionType,
                        Date = dateConverter.ConvertToDateTimeIso(p.Date),
                        TransactionId = p.TransactionId,
                    }).ToListAsync(cancellationToken);

            list.ForEach(l => {
                this.GetLedgerDetail(l);
            });

            var count = await _context.Ledgers.CountAsync(query, cancellationToken);
            var netBalance = await _context.Ledgers.SumAsync(query, p => p.Amount, cancellationToken);
            var firstLedger = list.FirstOrDefault();
            double previousBalance = 0;
            if (firstLedger != null)
            {
                var firstDate = DateTime.Parse(firstLedger.Date);
                previousBalance = await _context.Ledgers.SumAsync(
                    p => p.PartyId == request.PartyId && p.TransactionType == TransactionType.Party.ToInt() && p.Date < firstDate, p => p.Amount,
                    cancellationToken);
            }
            return new ResponseViewModel().CreateOk(new Response()
            {
                LedgerResponses = list,
                NetBalance = netBalance,
                PreviousBalance = previousBalance
            }, count);
        }

        public void GetLedgerDetail(LedgerResponse ledger)
        {
            if ((int)LedgerType.Purchase == ledger.LedgerType)
            {
                var purchase = _context.Purchases.GetBy(p => p.Id == ledger.Id, p => p.Include(pr => pr.GatePasses).ThenInclude(g => g.Product).Include(pr => pr.GatePasses).ThenInclude(g => g.Vehicle).Include(c => c.Charges));
                if (purchase != null)
                {
                    ledger.AdditionalCharges = purchase.Charges.Sum(c => c.Total);
                    ledger.BagQuantity = purchase.BagQuantity;
                    ledger.BoriQuantity = purchase.BoriQuantity;
                    ledger.Commission = purchase.Commission;
                    ledger.Freight = purchase.Freight;
                    ledger.GatepassIds = String.Join(", ", purchase.GatePasses.Select(c => c.Id));
                    ledger.Product = String.Join(", ", purchase.GatePasses.Select(c => c.Product.Name).Distinct());
                    ledger.LotNumber = String.Join(", ", purchase.GatePasses.Select(c => c.LotNumber).Distinct());
                    ledger.VehicleNo = String.Join(", ", purchase.GatePasses.Select(c => c.Vehicle.PlateNo).Distinct());
                    ledger.Broker = String.Join(", ", purchase.GatePasses.Select(c => c.Broker).Distinct());
                    ledger.NetWeight = purchase.GatePasses.Select(c => c.NetWeight).Sum();
                    ledger.InvoiceId = String.Join(", ", purchase.GatePasses.Select(c => c.PurchaseId).Distinct());
                    ledger.BiltyNumber = String.Join(", ", purchase.GatePasses.Select(c => c.BiltyNumber).Distinct());
                    ledger.TotalMaund = purchase.TotalMaund;
                    ledger.Rate = purchase.Rate;
                    ledger.RateBasedOn = purchase.RateBasedOn;
                }
            }
            else if ((int)LedgerType.Sale == ledger.LedgerType)
            {
                var sale = _context.Sales.GetBy(p => p.Id == ledger.Id, p => p.Include(pr => pr.GatePasses).ThenInclude(g => g.Product).Include(pr => pr.GatePasses).ThenInclude(g => g.Vehicle).Include(c => c.Charges));
                if (sale != null)
                {
                    ledger.AdditionalCharges = sale.Charges.Sum(c => c.Total);
                    ledger.BagQuantity = sale.BagQuantity;
                    ledger.BoriQuantity = sale.BoriQuantity;
                    ledger.Commission = sale.Commission;
                    ledger.Freight = sale.Freight;
                    ledger.GatepassIds = String.Join(", ", sale.GatePasses.Select(c => c.Id));
                    ledger.Product = String.Join(", ", sale.GatePasses.Select(c => c.Product.Name).Distinct());
                    ledger.VehicleNo = String.Join(", ", sale.GatePasses.Select(c => c.Vehicle.PlateNo).Distinct());
                    ledger.LotNumber = String.Join(", ", sale.GatePasses.Select(c => c.LotNumber).Distinct());
                    ledger.InvoiceId = String.Join(", ", sale.GatePasses.Select(c => c.PurchaseId).Distinct());
                    ledger.NetWeight = sale.GatePasses.Select(c => c.NetWeight).Sum();
                    ledger.Broker = String.Join(", ", sale.GatePasses.Select(c => c.Broker).Distinct());
                    ledger.BiltyNumber = String.Join(", ", sale.GatePasses.Select(c => c.BiltyNumber).Distinct());
                    ledger.TotalMaund = sale.TotalMaund;
                    ledger.Rate = sale.Rate;
                    ledger.RateBasedOn = sale.RateBasedOn;
                }
            }
        }

        class Response
        {
            public List<LedgerResponse> LedgerResponses { get; set; }
            public double NetBalance { get; set; }
            public double PreviousBalance { get; set; }
        }
        public class LedgerResponse
        {
            public int Id { get; set; }
            public int LedgerType { get; set; }
            public int TransactionType { get; set; }
            public double Amount { get; set; }
            public double Freight { get; set; }

            public int PartyId { get; set; }
            public Party Party { get; set; }
            public string VehicleNo { get; set; }
            public string InvoiceId { get; set; }
            public string LotNumber { get; set; }
            public double NetWeight { get; set; }
            public string Broker { get; set; }
            public string BiltyNumber { get; set; }
            public string Date { get; set; }
            public string TransactionId { get; set; }
            public string Product { get; set; }
            public double BoriQuantity { get; set; }
            public double BagQuantity { get; set; }
            public double TotalMaund { get; set; }
            public double AdditionalCharges { get; set; }
            public double Commission { get; set; }
            public string GatepassIds { get; set; }
            public RateBasedOn RateBasedOn { get; set; }
            public double Rate { get; set; }

        }
    }
}