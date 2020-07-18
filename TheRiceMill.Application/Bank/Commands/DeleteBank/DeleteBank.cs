using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Bank.Commands.DeleteBank
{

    public class DeleteBankRequestHandler : IRequestHandler<DeleteBankRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteBankRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteBankRequestModel request, CancellationToken cancellationToken)
        {
            var bank = _context.Banks.GetBy(p => p.Id == request.BankId && !p.BankAccounts.Any());
            if (bank == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Bank),request.BankId);
            }
            _context.Remove(bank);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel();
        }
    }

}