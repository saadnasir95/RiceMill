using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.DeleteBankAccount
{

    public class DeleteBankAccountRequestHandler : IRequestHandler<DeleteBankAccountRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteBankAccountRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteBankAccountRequestModel request, CancellationToken cancellationToken)
        {
            var bankAccount = _context.BankAccounts.GetBy(p => p.Id == request.Id && !p.BankTransactions.Any());
            if (bankAccount == null)
            {
                throw new CannotDeleteException(nameof(BankAccount),request.Id);
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }
    }

}