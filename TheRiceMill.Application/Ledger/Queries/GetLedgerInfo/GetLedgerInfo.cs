using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgerInfo
{
    public class GetLedgerInfoRequestHandler : IRequestHandler<GetLedgerInfoRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetLedgerInfoRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetLedgerInfoRequestModel request,
            CancellationToken cancellationToken)
        {
            switch (request.LedgerType)
            {
                case LedgerType.Sale:
                    //var sale = _context.Sales.GetBy(p => p.Id == request.Id,
                    //    p => p.Include(pr => pr.Product));
                    //if (sale != null)
                    //{
                    //    return new ResponseViewModel().CreateOk(new SaleInfo()
                    //    {
                    //        Product = sale.Product.Name,
                    //        MaundPrice = sale.RatePerMaund,
                    //        TotalActualBagWeight = sale.TotalActualBagWeight,
                    //        TotalMaund = sale.TotalMaund
                    //    });
                    //}

                    break;
                case LedgerType.Purchase:
                    //var purchase = _context.Purchases.GetBy(p => p.Id == request.Id,
                    //    p => p.Include(pr => pr.GatePasses));
                    //if (purchase != null)
                    //{
                    //    return new ResponseViewModel().CreateOk(new PurchaseInfo()
                    //    {
                    //        //Product = purchase.Product.Name,
                    //        //MaundPrice = purchase.Rate,
                    //        //TotalActualBagWeight = purchase,
                    //        //TotalMaund = purchase.TotalMaund
                    //    });
                    //}

                    break;
                case LedgerType.BankTransaction:
                    var bankTransaction = _context.BankTransactions.GetBy(p => p.Id == request.Id,
                        p => p.Include(pr => pr.BankAccount.Bank));
                    if (bankTransaction != null)
                    {
                        return new ResponseViewModel().CreateOk(new BankInfo()
                        {
                            Bank = bankTransaction.BankAccount.Bank.Name,
                            AccountNumber = bankTransaction.BankAccount.AccountNumber,
                            ChequeNumber = bankTransaction.ChequeNumber,
                            PaymentType = bankTransaction.PaymentType
                        });
                    }
                    break;
            }

            return new ResponseViewModel().CreateOk(new { });
        }

        class SaleInfo
        {
            public string Product { get; set; }
            public double TotalActualBagWeight { get; set; }
            public double MaundPrice { get; set; }
            public double TotalMaund { get; set; }
        }

        class PurchaseInfo
        {
            public string Product { get; set; }
            public double TotalActualBagWeight { get; set; }
            public double TotalMaund { get; set; }
            public double MaundPrice { get; set; }
        }

        class BankInfo
        {
            public int PaymentType { get; set; }
            public string AccountNumber { get; set; }
            public string Bank { get; set; }
            public string ChequeNumber { get; set; }
        }
    }
}