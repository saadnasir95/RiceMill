using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;
using static TheRiceMill.Application.Ledger.Queries.GetLedgers.GetPartyLedgerRequestHandler;

namespace TheRiceMill.Application.Ledger.Queries.GetCompanyLedger
{
    class GetCompanyLedgerRequestHandler : IRequestHandler<GetCompanyLedgerRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetCompanyLedgerRequestHandler(TheRiceMillDbContext context)
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
        public async Task<ResponseViewModel> Handle(GetCompanyLedgerRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Ledger, bool>> query ;
            query = CreateQuery(request);
            
            var dateConverter = new DateConverter();
            var list = await _context.Ledgers
                .GetManyReadOnly(query, "Date", request.Page, request.PageSize, false,
                    p => p.Include(pr => pr.Party)).Select(p =>
                    new LedgerResponse()
                    {
                        Id = p.Id,
                        PartyId = p.PartyId,
                        LedgerType = p.LedgerType,
                        Amount = p.Amount,
                        Party = p.Party,
                        TransactionType = p.TransactionType,
                        Date = dateConverter.ConvertToDateTimeIso(p.Date),
                        TransactionId = p.TransactionId,
                    }).ToListAsync(cancellationToken);
            var count = await _context.Ledgers.CountAsync(query, cancellationToken);
            var netBalance = await _context.Ledgers.SumAsync(query, p => p.Amount, cancellationToken);
            var firstLedger = list.FirstOrDefault();
            double previousBalance = 0;
            if (firstLedger != null)
            {
                var firstDate = DateTime.Parse(firstLedger.Date);
                previousBalance = await _context.Ledgers.SumAsync(
                    p => p.TransactionType == TransactionType.Company.ToInt() && p.Date < firstDate, p => p.Amount,
                    cancellationToken);
            }
            return new ResponseViewModel().CreateOk(new Response()
            {
                LedgerResponses = list,
                NetBalance = netBalance,
                PreviousBalance = previousBalance
            }, count);
        }

        private Expression<Func<Domain.Entities.Ledger, bool>> CreateQuery(GetCompanyLedgerRequestModel request)
        {
            if (request.ToDate != null && request.FromDate != null && request.LedgerType != 0)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.LedgerType == request.LedgerType && p.Date >= request.FromDate && p.Date <= request.ToDate;
            }
            if (request.ToDate != null && request.FromDate != null)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.Date >= request.FromDate && p.Date <= request.ToDate;
            }
            if (request.LedgerType != 0)
            {
                return p => p.TransactionType == TransactionType.Company.ToInt() && p.LedgerType == request.LedgerType;
            }
            else
            {
                return p => p.TransactionType == TransactionType.Company.ToInt();
            }
        }

        class Response
        {
            public List<LedgerResponse> LedgerResponses { get; set; }
            public double NetBalance { get; set; }
            public double PreviousBalance { get; set; }
        }
    }
}
