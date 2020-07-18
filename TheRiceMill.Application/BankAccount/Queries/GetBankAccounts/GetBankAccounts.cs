using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.BankAccount.Queries.GetBankAccounts
{

    public class GetBankAccountsRequestHandler : IRequestHandler<GetBankAccountsRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetBankAccountsRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetBankAccountsRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var dateConverter = new DateConverter();
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.BankAccount, bool>> query = p =>
                p.Bank.Name.Contains(request.Search) ||
                p.AccountNumber.Contains(request.Search);
            
            
        var list = await _context.BankAccounts.GetManyReadOnly(query, request.OrderBy, request.Page,
                request.PageSize,
                request.IsDescending, p => p.Include(pr => pr.Bank)).Select(p =>
                new Response()
                {
                    Id = p.Id,
                    Bank = p.Bank.Name,
                    AccountNumber = p.AccountNumber,
                    CurrentBalance = p.CurrentBalance,
                    CreatedDate = dateConverter.ConvertToDateTimeIso(p.CreatedDate),
                    BankId = p.BankId,
                    TotalCredit = p.BankTransactions.Sum(pr => pr.Credit),
                    TotalDebit = p.BankTransactions.Sum(pr => pr.Debit),
                    TotalTransactions = p.BankTransactions.Count()
                }).ToListAsync(cancellationToken);
            var count = await _context.BankAccounts.CountAsync(query, cancellationToken);
            return new ResponseViewModel().CreateOk(list, count);
        }

        class Response
        {
            public int BankId { get; set; }
            public string Bank { get; set; }
            public string AccountNumber { get; set; }
            public double CurrentBalance { get; set; }
            public double TotalDebit { get; set; }
            public double TotalCredit { get; set; }
            public int TotalTransactions { get; set; }
            public int Id { get; set; }
            public string CreatedDate { get; set; }
        }
    }

}