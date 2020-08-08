using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.DeleteBankTransaction
{

    public class DeleteBankTransactionRequestHandler : IRequestHandler<DeleteBankTransactionRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteBankTransactionRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteBankTransactionRequestModel request, CancellationToken cancellationToken)
        {
            var bankTransaction = _context.BankTransactions.GetBy(p => p.Id == request.BankTransactionId);
            if (bankTransaction == null)
            {
                throw new NotFoundException(nameof(BankTransaction),request.BankTransactionId);
            }
            
            var ledger = _context.Ledgers.GetBy(p => p.Id == request.BankTransactionId && p.LedgerType == (int)LedgerType.BankTransaction);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger),request.BankTransactionId);
            }

            _context.Remove(ledger);
            _context.Remove(bankTransaction);

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }
    }

}