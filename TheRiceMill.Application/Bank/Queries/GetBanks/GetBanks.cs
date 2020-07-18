using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Bank.Queries.GetBanks
{

    public class GetBanksRequestHandler : IRequestHandler<GetBanksRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetBanksRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetBanksRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = await _context.Banks.GetMany(p => p.Name.Contains(request.Search), request.OrderBy, request.Page,
                request.PageSize, request.IsDescending).Select(p => new Response()
            {
                Name = p.Name,
                Id = p.Id,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(p.CreatedDate),
            }).ToListAsync(cancellationToken);
            var count = await _context.Banks.CountAsync(p => p.Name.Contains(request.Search),cancellationToken);
            return new ResponseViewModel().CreateOk(list,count);
        }

        class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string CreatedDate { get; set; }
        }
    }

}