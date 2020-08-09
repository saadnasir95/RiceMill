using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Ledger.Queries.GetLedgers
{
    public class GetPartyLedgerRequestHandler : IRequestHandler<GetPartyLedgerRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPartyLedgerRequestHandler(TheRiceMillDbContext context)
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
        public async Task<ResponseViewModel> Handle(GetPartyLedgerRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Ledger, bool>> query = p => p.PartyId == request.PartyId && p.TransactionType == TransactionType.Party.ToInt();

            //PrevBalance = 10000

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
                    p => p.PartyId == request.PartyId && p.TransactionType == TransactionType.Party.ToInt() && p.Date < firstDate, p => p.Amount,
                    cancellationToken);
            }
            return new ResponseViewModel().CreateOk(new Response()
            {
                LedgerResponses = list,
                NetBalance = netBalance,
                PreviousBalance = previousBalance
            }, count);
        }

        class Response
        {
            public List<LedgerResponse> LedgerResponses { get; set; }
            public double NetBalance { get; set; }
            public double PreviousBalance { get; set; }
        }
        public class LedgerResponse
        {
            public int Id { get; set; }
            public int LedgerType { get; set; }
            public int TransactionType { get; set; }
            public double Amount { get; set; }
            public int PartyId { get; set; }
            public Party Party { get; set; }
            public string Date { get; set; }
            public string TransactionId { get; set; }
        }
    }
}