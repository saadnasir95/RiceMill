using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Companies.Queries
{
    public class GetCompanyRequestHandler : IRequestHandler<GetCompanyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetCompanyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetCompanyRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = _context.Companies.GetMany(p => p.Name.Contains(request.Search), request.OrderBy, request.Page,
                request.PageSize, request.IsDescending).Select(company => new CompanyInfoResponseModel()
            {
                Name = company.Name,
                Id = company.Id,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(company.CreatedDate),
            }).ToList();
            var count = _context.Companies.Count(p => p.Name.Contains(request.Search));
            return Task.FromResult(new ResponseViewModel().CreateOk(list, count));
        }
    }
}