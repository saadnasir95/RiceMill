using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Lots.Queries
{
    class GetYearsRequestHandler : IRequestHandler<GetYearRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetYearsRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetYearRequestModel request, CancellationToken cancellationToken)
        {

            var yearList = _context.Lots.Select(q => new List<string>() {
                q.Year.ToString()
            });
            return new ResponseViewModel().CreateOk(yearList);
        }
    }
}
