using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Companies.Queries
{
    public class GetPartyRequestHandler : IRequestHandler<GetPartyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPartyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetPartyRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = _context.Parties.GetMany(p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt(), request.OrderBy, request.Page,
                request.PageSize, request.IsDescending).Select(party => new PartyInfoResponseModel()
                {
                    Name = party.Name,
                    Id = party.Id,
                    Address = party.Address,
                    PhoneNumber = party.PhoneNumber,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(party.CreatedDate),
                    CompanyId = (CompanyType)party.CompanyId
                }).ToList();
            var count = _context.Parties.Count(p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt());
            return Task.FromResult(new ResponseViewModel().CreateOk(list, count));
        }
    }
}