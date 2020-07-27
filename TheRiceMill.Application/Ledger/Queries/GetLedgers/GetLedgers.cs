using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgers
{
    public class GetLedgersRequestHandler : IRequestHandler<GetLedgersRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetLedgersRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///L1 C1000 D0 = 1000
        ///L2 C1000 D0 = 2000
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseViewModel> Handle(GetLedgersRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Ledger, bool>> query = p => p.PartyId == request.CompanyId;

            //PrevBalance = 10000

            var dateConverter = new DateConverter();
            var list = await _context.Ledgers
                .GetManyReadOnly(query, "CreatedDate", request.Page, request.PageSize, false,
                    p => p.Include(pr => pr.Party)).Select(p =>
                    new LedgerResponse()
                    {
                        CompanyId = p.PartyId,
                        LedgerType = p.LedgerType,
                        Credit = p.Credit,
                        Debit = p.Debit,
                        Description = p.Description,
                        CompanyName = p.Party.Name,
                        CreatedDate = dateConverter.ConvertToDateTimeIso(p.CreatedDate),
                        TransactionId = p.TransactionId,
                    }).ToListAsync(cancellationToken);
            var count = await _context.Ledgers.CountAsync(query, cancellationToken);
            var totalCredit = await _context.Ledgers.SumAsync(query,p => p.Credit, cancellationToken);
            var totalDebit = await _context.Ledgers.SumAsync(query,p => p.Debit, cancellationToken);
            var firstLedger = list.FirstOrDefault();
            double previousBalance = 0;
            if (firstLedger != null)
            {
                var firstDate = DateTime.Parse(firstLedger.CreatedDate);
                previousBalance = await _context.Ledgers.SumAsync(
                    p => p.PartyId == request.CompanyId && p.CreatedDate < firstDate, p => p.Credit - p.Debit,
                    cancellationToken);
            }
            return new ResponseViewModel().CreateOk(new Response()
            {
                LedgerResponses = list,
                TotalCredit = totalCredit,
                TotalDebit = totalDebit,
                PreviousBalance = previousBalance
            }, count);
        }

        class Response
        {
            public List<LedgerResponse> LedgerResponses { get; set; }
            public double TotalCredit { get; set; }
            public double TotalDebit { get; set; }
            public double PreviousBalance { get; set; }
        }
        class LedgerResponse
        {
            public double Debit { get; set; }
            public double Credit { get; set; }
            public string Description { get; set; }
            public int CompanyId { get; set; }
            public string CompanyName { get; set; }
            public int LedgerType { get; set; }
            public string CreatedDate { get; set; }
            public int TransactionId { get; set; }
        }
    }
}