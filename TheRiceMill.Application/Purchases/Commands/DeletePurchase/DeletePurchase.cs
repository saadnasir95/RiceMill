using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
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
            var purchase = _context.Purchases.GetBy(p => p.Id == request.PurchaseId, p => p.Include(pr => pr.Charges));
            if (purchase == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Purchase),request.PurchaseId);
            }
            /*var ledger = _context.Ledgers.GetBy(p => p.TransactionId == request.PurchaseId && p.LedgerType == (int)LedgerType.Purchase);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger),request.PurchaseId);
            }*/
            if(purchase.Charges.Count > 0)
            {
                _context.Charges.RemoveRange(purchase.Charges);
            }
            _context.Purchases.Remove(purchase);
            //_context.Ledgers.Remove(ledger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }
    }

}