using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Companies.Commands
{
    public class UpdatePartyRequestHandler : IRequestHandler<UpdatePartyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdatePartyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdatePartyRequestModel request, CancellationToken cancellationToken)
        {
            var party = await _context.Parties.FindAsync(request.Id);
            if (party == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            if (!party.NormalizedName.Equals(request.Name.ToUpper()))
            {
                if (_context.Parties.Any(p => p.NormalizedName.Equals(request.Name.ToUpper())))
                {
                    throw new AlreadyExistsException(nameof(Product), nameof(request.Name), request.Name);
                }
            }

            party.Name = request.Name;
            party.NormalizedName = request.Name.ToUpper();
            party.Address = request.Address;
            party.PhoneNumber = request.PhoneNumber;
            party.CompanyId = request.CompanyId.ToInt();
            _context.Parties.Update(party);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new PartyInfoResponseModel()
            {
                Name = party.Name,
                Id = party.Id,
                Address = party.Address,
                PhoneNumber = party.PhoneNumber,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(party.CreatedDate),
                CompanyId = (CompanyType)party.CompanyId
            });
        }
    }
}