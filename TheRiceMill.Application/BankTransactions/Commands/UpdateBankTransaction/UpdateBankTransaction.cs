using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.UpdateBankTransaction
{

    public class UpdateBankTransactionRequestHandler : IRequestHandler<UpdateBankTransactionRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateBankTransactionRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateBankTransactionRequestModel request, CancellationToken cancellationToken)
        {
            var bankAccount = _context.BankAccounts.GetByReadOnly(p => p.Id == request.BankAccountId, p => p.Include(pr => pr.Bank));
            if (bankAccount == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.BankAccount), request.BankAccountId);
            }
            var company = _context.Parties.GetByReadOnly(p => p.Id == request.CompanyId);
            if (company == null)
            {
                throw new NotFoundException(nameof(Party), request.CompanyId);
            }

            var bankTransaction = _context.BankTransactions.GetBy(p => p.Id == request.Id);
            if (bankTransaction == null)
            {
                throw new NotFoundException(nameof(BankTransaction),request.Id);
            }
            
            var ledger = _context.Ledgers.GetBy(p => p.TransactionId == request.Id && p.LedgerType == (int)LedgerType.BankTransaction);
            if (ledger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger),request.Id);
            }
            
            bankTransaction.BankAccountId = request.BankAccountId;
            bankTransaction.PartyId = request.CompanyId;
            bankTransaction.Credit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0;
            bankTransaction.Debit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0;
            bankTransaction.TransactionDate = request.TransactionDate;
            bankTransaction.TransactionType = (int) request.TransactionType;
            bankTransaction.ChequeNumber = request.ChequeNumber;
            bankTransaction.PaymentType = (int) request.PaymentType;
            ledger.PartyId = request.CompanyId;
            ledger.Credit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0;
            ledger.Debit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0;
            ledger.LedgerType = (int) LedgerType.BankTransaction;
            ledger.Description = "";

            _context.Update(ledger);
            _context.Update(bankTransaction);
            await _context.SaveChangesAsync(cancellationToken);
            var dateConverter = new DateConverter();
            return new ResponseViewModel().CreateOk(new Response()
            {
                BankAccountId = request.BankAccountId,
                AccountNumber = bankAccount.AccountNumber,
                CompanyId = request.CompanyId,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = dateConverter.ConvertToDateTimeIso(request.TransactionDate),
                TransactionType = (int) request.TransactionType,
                CompanyName = company.Name,
                CreatedDate = dateConverter.ConvertToDateTimeIso(bankTransaction.CreatedDate),
                BankName = bankAccount.Bank.Name
            });
        }

        class Response
        {
            public int BankAccountId { get; set; }
            public string BankName { get; set; }
            public double TransactionAmount { get; set; }
            public int TransactionType { get; set; }
            public string TransactionDate { get; set; }
            public string AccountNumber { get; set; }
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string CreatedDate { get; set; }
        }
    }

}