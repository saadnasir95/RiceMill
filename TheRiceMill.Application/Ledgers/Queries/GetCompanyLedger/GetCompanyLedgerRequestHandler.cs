using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Ledgers.Queries.GetCompanyLedger;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;
using static TheRiceMill.Application.Ledgers.Queries.GetLedgerInfo.GetLedgerInfoRequestHandler;
using static TheRiceMill.Application.Ledgers.Queries.GetLedgers.GetPartyLedgerRequestHandler;

namespace TheRiceMill.Application.Ledger.Queries.GetCompanyLedger
{
    class GetCompanyLedgerRequestHandler : IRequestHandler<GetCompanyLedgerRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetCompanyLedgerRequestHandler(TheRiceMillDbContext context)
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
        public async Task<ResponseViewModel> Handle(GetCompanyLedgerRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Ledger, bool>> query;
            query = CreateQuery(request);

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

            list.ForEach(l =>
            {
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
                    p => p.TransactionType == TransactionType.Company.ToInt() && p.Date < firstDate, p => p.Amount,
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
                    ledger.VehicleNo = String.Join(", ", purchase.GatePasses.Select(c => c.Vehicle.PlateNo).Distinct());
                    ledger.Broker = String.Join(", ", purchase.GatePasses.Select(c => c.Broker).Distinct());
                    ledger.NetWeight = purchase.GatePasses.Select(c => c.NetWeight).Sum();
                    ledger.InvoiceId = String.Join(", ", purchase.GatePasses.Select(c => c.PurchaseId).Distinct());
                    //ledger.LotNumber = String.Join(", ", purchase.GatePasses.Select(c => c.LotNumber).Distinct());
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
                    ledger.InvoiceId = String.Join(", ", sale.GatePasses.Select(c => c.PurchaseId).Distinct());
                    ledger.NetWeight = sale.GatePasses.Select(c => c.NetWeight).Sum();
                    ledger.Broker = String.Join(", ", sale.GatePasses.Select(c => c.Broker).Distinct());
                    //ledger.LotNumber = String.Join(", ", sale.GatePasses.Select(c => c.LotNumber).Distinct());
                    ledger.BiltyNumber = String.Join(", ", sale.GatePasses.Select(c => c.BiltyNumber).Distinct());
                    ledger.TotalMaund = sale.TotalMaund;
                    ledger.Rate = sale.Rate;
                    ledger.RateBasedOn = sale.RateBasedOn;
                }
            }
        }
        private Expression<Func<Domain.Entities.Ledger, bool>> CreateQuery(GetCompanyLedgerRequestModel request)
        {
            if (request.ToDate != null && request.FromDate != null && request.LedgerType != 0)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.LedgerType == request.LedgerType && p.Date >= request.FromDate && p.Date <= request.ToDate && p.CompanyId == request.CompanyId.ToInt();
            }
            if (request.ToDate != null && request.FromDate != null)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.Date >= request.FromDate && p.Date <= request.ToDate && p.CompanyId == request.CompanyId.ToInt();
            }
            if (request.LedgerType != 0)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.LedgerType == request.LedgerType && p.CompanyId == request.CompanyId.ToInt();
            }
            else
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.CompanyId == request.CompanyId.ToInt();
            }
        }

        class Response
        {
            public List<LedgerResponse> LedgerResponses { get; set; }
            public double NetBalance { get; set; }
            public double PreviousBalance { get; set; }
        }
    }
}
