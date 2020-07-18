using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankAccount.Commands.CreateBankAccount
{

    public class CreateBankAccountRequestHandler : IRequestHandler<CreateBankAccountRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateBankAccountRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateBankAccountRequestModel request, CancellationToken cancellationToken)
        {
            var bank = _context.Banks.GetBy(p => p.Id == request.BankId);
            if (bank == null)
            {
                throw new NotFoundException(nameof(Bank),request.BankId);
            }
            var bankAccount = new Domain.Entities.BankAccount()
            {
                BankId = request.BankId,
                AccountNumber = request.AccountNumber,
                CurrentBalance = request.CurrentBalance
            };

            _context.BankAccounts.Add(bankAccount);
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