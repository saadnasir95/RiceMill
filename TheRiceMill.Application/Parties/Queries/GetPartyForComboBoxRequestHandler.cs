using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Companies.Queries
{
    public class GetPartyForComboBoxRequestHandler : IRequestHandler<GetPartyForComboBoxRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPartyForComboBoxRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetPartyForComboBoxRequestModel request, CancellationToken cancellationToken)
        {
            var list = _context.Parties.GetMany(p => p.Name.Contains(request.Search), p => p.Name, 1,
                request.PageSize, true).Select(company => new PartyInfoComboBoxResponseModel()
            {
                Name = company.Name,
                Id = company.Id,
                PhoneNumber = company.PhoneNumber,
            }).ToList();
            return Task.FromResult(new ResponseViewModel().CreateOk(list));
        }
    }
}