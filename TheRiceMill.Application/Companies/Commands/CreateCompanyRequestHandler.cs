using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Companies.Commands
{
    public class CreateCompanyRequestHandler : IRequestHandler<CreateCompanyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateCompanyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateCompanyRequestModel request, CancellationToken cancellationToken)
        {
            if (_context.Companies.Any(p => p.NormalizedName.Equals(request.Name.ToUpper())))
            {
                throw new AlreadyExistsException(nameof(Company), nameof(request.Name), request.Name);
            }
            var company = new Company()
            {
                Name = request.Name,
                NormalizedName = request.Name.ToUpper(),
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
            };
            _context.Add(company);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new CompanyInfoResponseModel()
            {
                Name = company.Name,
                Id = company.Id,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(company.CreatedDate),
                Address = company.Address,
                PhoneNumber = company.PhoneNumber
            });
        }
    }
}