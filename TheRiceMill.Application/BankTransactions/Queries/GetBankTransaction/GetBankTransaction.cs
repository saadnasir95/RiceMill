using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankTransactions.Queries.GetBankTransaction
{
    public class GetBankTransactionRequestHandler : IRequestHandler<GetBankTransactionRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetBankTransactionRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetBankTransactionRequestModel request,
            CancellationToken cancellationToken)
        {
            var dateConverter = new DateConverter();
            request.SetDefaultValue();
            Expression<Func<BankTransaction, bool>> query = p => true;
            if (request.BankAccountId > 0)
            {
                query = query.AndAlso(p => p.BankAccountId == request.BankAccountId);
            }


            var bankAccount = _context.BankAccounts.GetBy(p => p.Id == request.BankAccountId);
            if (bankAccount == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.BankAccount), request.BankAccountId);
            }
            var currentBalance = _context.BankTransactions.Where(p => p.BankAccountId == request.BankAccountId).Sum(p => p.Credit - p.Debit) + bankAccount.CurrentBalance;
            var list = await _context.BankTransactions.GetManyReadOnly(query, "CreatedDate", request.Page,
                request.PageSize,
                true, p => p.Include(pr => pr.BankAccount.Bank).Include(pr => pr.Party)).Select(p =>
                new Info()
                {
                    Id = p.Id,
                    BankId = p.BankAccount.BankId,
                    Company = new CompanyRequestModel()
                    {
                        Address = p.Party.Address,
                        Name = p.Party.Name,
                        PhoneNumber = p.Party.PhoneNumber,
                    },
                    BankAccountId = p.BankAccount.Id,
                    CompanyId = p.PartyId,
                    TransactionDate = dateConverter.ConvertToDateTimeIso(p.TransactionDate),
                    TransactionType = p.TransactionType,
                    ChequeNumber = p.ChequeNumber,
                    PaymentType = p.PaymentType,
                    Credit = p.Credit,
                    Debit = p.Debit,
                }).ToListAsync(cancellationToken);
            var count = await _context.BankTransactions.CountAsync(query, cancellationToken);
            var firstId = 0;
            var response = new Response()
            {
                Transactions = list,
                NetBalance = currentBalance,
                PreviousBalance = 0
            };
            if (list.Any())
            {
                firstId = list.First().Id;
                var totalCredit = _context.BankTransactions.Where(p => p.Id < firstId).Sum(p => p.Credit - p.Debit);
                response.PreviousBalance = totalCredit;
            }

            return new ResponseViewModel().CreateOk(response, count);
        }

        class Response
        {
            public List<Info> Transactions { get; set; }
            public double NetBalance { get; set; }
            public double PreviousBalance { get; set; }
        }

        class Info
        {
            public int BankId { get; set; }
            public double Credit { get; set; }
            public double Debit { get; set; }
            public int TransactionType { get; set; }
            public string TransactionDate { get; set; }
            public int CompanyId { get; set; }
            public CompanyRequestModel Company { get; set; }
            public int Id { get; set; }
            public int BankAccountId { get; set; }
            public string ChequeNumber { get; set; }
            public int PaymentType { get; set; }
        }
    }
}