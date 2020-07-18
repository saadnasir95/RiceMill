using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.UpdateBankAccount
{

    public class UpdateBankAccountRequestHandler : IRequestHandler<UpdateBankAccountRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateBankAccountRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateBankAccountRequestModel request, CancellationToken cancellationToken)
        {
            var bankAccount = _context.BankAccounts.GetBy(p => p.Id == request.Id);
            if (bankAccount == null)
            {
                throw new NotFoundException(nameof(BankAccount),request.Id);
            }
            var bank = _context.Banks.GetBy(p => p.Id == request.BankId);
            if (bank == null)
            {
                throw new NotFoundException(nameof(Bank),request.BankId);
            }

            bankAccount.BankId = request.BankId;
            bankAccount.AccountNumber = request.AccountNumber;
            bankAccount.CurrentBalance = request.CurrentBalance;

            _context.Update(bankAccount);

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new Response()
            {
                Id = bankAccount.Id,
                AccountNumber = request.AccountNumber,
                BankId = request.BankId,
                BankName = bank.Name,
                CurrentBalance = request.CurrentBalance
            });
        }

        class Response
        {
            public int BankId { get; set; }
            public string BankName { get; set; }
            public string AccountNumber { get; set; }
            public double CurrentBalance { get; set; }
            public int Id { get; set; }
        }
    }

}