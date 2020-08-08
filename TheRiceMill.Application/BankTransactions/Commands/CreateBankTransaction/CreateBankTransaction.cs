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
            var party = _context.Parties.GetByReadOnly(p => p.Id == request.PartyId);
            if (party == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Party), request.PartyId);
            }

            var bankTransaction = new BankTransaction()
            {
                BankAccountId = request.BankAccountId,
                PartyId = request.PartyId,
                //Credit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0,
                //Debit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0,
                TransactionDate = request.TransactionDate,
                TransactionType = (int) request.TransactionType,
                ChequeNumber = request.ChequeNumber,
                PaymentType = (int)request.PaymentType
            };
            _context.Add(bankTransaction);
            await _context.SaveChangesAsync(cancellationToken);
            var ledger = new Domain.Entities.Ledger()
            {
                PartyId = request.PartyId,
                //Credit = request.TransactionType == TransactionType.Credit ? request.TransactionAmount : 0,
                //Debit = request.TransactionType == TransactionType.Debit ? request.TransactionAmount : 0,
                LedgerType = (int) LedgerType.BankTransaction,
                Id = bankTransaction.Id,
            };
            _context.Add(ledger);
            var dateConverter = new DateConverter();
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new Response()
            {
                BankAccountId = request.BankAccountId,
                AccountNumber = bankAccount.AccountNumber,
                PartyId = request.PartyId,
                TransactionAmount = request.TransactionAmount,
                TransactionDate = dateConverter.ConvertToDateTimeIso(request.TransactionDate),
                TransactionType = (int) request.TransactionType,
                PartyName = party.Name,
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
            public int PartyId { get; set; }
            public string PartyName { get; set; }
            public string CreatedDate { get; set; }
        }
    }
}