using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankTransactions.Commands.CreateBankTransaction
{
    public class
        CreateBankTransactionRequestHandler : IRequestHandler<CreateBankTransactionRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateBankTransactionRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateBankTransactionRequestModel request,
            CancellationToken cancellationToken)
        {
            var bankAccount = _context.BankAccounts.GetByReadOnly(p => p.Id == request.BankAccountId, p => p.Include(pr => pr.Bank));
            if (bankAccount == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Bank), request.BankAccountId);
            }
            var company = _context.Companies.GetByReadOnly(p => p.Id == request.CompanyId);
            if (company == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Company), request.CompanyId);
            }

            var bankTransaction = new BankTransaction()
            {
                BankAccountId = request.BankAccountId,
                CompanyId = request.CompanyId,
                Credit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0,
                Debit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0,
                TransactionDate = request.TransactionDate,
                TransactionType = (int) request.TransactionType,
                ChequeNumber = request.ChequeNumber,
                PaymentType = (int)request.PaymentType
            };
            _context.Add(bankTransaction);
            await _context.SaveChangesAsync(cancellationToken);
            var ledger = new Domain.Entities.Ledger()
            {
                CompanyId = request.CompanyId,
                Credit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0,
                Debit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0,
                LedgerType = (int) LedgerType.BankTransaction,
                Description = "",
                TransactionId = bankTransaction.Id,
            };
            _context.Add(ledger);
            var dateConverter = new DateConverter();
            await _context.SaveChangesAsync(cancellationToken);
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