using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Bank.Queries.GetAllBanks
{

    public class GetAllBanksRequestHandler : IRequestHandler<GetAllBanksRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetAllBanksRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetAllBanksRequestModel request, CancellationToken cancellationToken)
        {
            var list = await _context.Banks.Select(p => new Response()
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(list);
        }

        class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }

}