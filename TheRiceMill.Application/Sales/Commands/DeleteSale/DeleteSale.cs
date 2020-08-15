using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sales.Commands.DeleteSale
{

    public class DeleteSaleRequestHandler : IRequestHandler<DeleteSaleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteSaleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteSaleRequestModel request, CancellationToken cancellationToken)
        {
            var sale = _context.Sales.GetBy(p => p.Id == request.SaleId, p => p.Include(pr => pr.Charges));
            if (sale == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Sale),request.SaleId);
            }
            var ledger = _context.Ledgers.GetBy(p => p.Id == request.SaleId && p.LedgerType == (int)LedgerType.Sale);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger),request.SaleId);
            }
            _context.Charges.RemoveRange(sale.Charges);
            _context.Sales.Remove(sale);
            _context.Ledgers.Remove(ledger);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel();
        }
    }

}