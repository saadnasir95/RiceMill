using System.Linq;
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
    public class UpdateCompanyRequestHandler : IRequestHandler<UpdateCompanyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateCompanyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateCompanyRequestModel request, CancellationToken cancellationToken)
        {
            var company = await _context.Parties.FindAsync(request.Id);
            if (company == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            if (!company.NormalizedName.Equals(request.Name.ToUpper()))
            {
                if (_context.Parties.Any(p => p.NormalizedName.Equals(request.Name.ToUpper())))
                {
                    throw new AlreadyExistsException(nameof(Product), nameof(request.Name), request.Name);
                }
            }

            company.Name = request.Name;
            company.NormalizedName = request.Name.ToUpper();
            company.Address = request.Address;
            company.PhoneNumber = request.PhoneNumber;
            _context.Parties.Update(company);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new CompanyInfoResponseModel()
            {
                Name = company.Name,
                Id = company.Id,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(company.CreatedDate),
            });
        }
    }
}