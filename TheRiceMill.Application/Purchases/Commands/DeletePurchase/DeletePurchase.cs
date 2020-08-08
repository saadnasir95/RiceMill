using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Purchases.Commands.DeletePurchase
{

    public class DeletePurchaseRequestHandler : IRequestHandler<DeletePurchaseRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeletePurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeletePurchaseRequestModel request, CancellationToken cancellationToken)
        {
            var purchase = _context.Purchases.GetBy(p => p.Id == request.PurchaseId, p => p.Include(pr => pr.Charges).Include(pr => pr.GatePasses));
            if (purchase == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Purchase), request.PurchaseId);
            }
            var partyledger = _context.Ledgers.GetBy(p => p.Id == request.PurchaseId && p.LedgerType == (int)LedgerType.Purchase && p.TransactionType == TransactionType.Party.ToInt());
            var companyLedger = _context.Ledgers.GetBy(p => p.Id == request.PurchaseId && p.LedgerType == (int)LedgerType.Purchase && p.TransactionType == TransactionType.Company.ToInt());
            if (partyledger == null || companyLedger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger), request.PurchaseId);
            }
            if (purchase.Charges.Count > 0)
            {
                _context.Charges.RemoveRange(purchase.Charges);
            }
            if (purchase.GatePasses?.Count > 0)
            {
                purchase.GatePasses.ForEach(pr => pr.PurchaseId = null);
            }
            _context.Purchases.Remove(purchase);
            //_context.Ledgers.Remove(ledger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }
    }

}