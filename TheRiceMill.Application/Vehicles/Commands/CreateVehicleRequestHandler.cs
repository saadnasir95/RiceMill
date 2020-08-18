using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Vehicles.Commands
{

    public class CreateVehicleRequestHandler : IRequestHandler<CreateVehicleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateVehicleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateVehicleRequestModel request, CancellationToken cancellationToken)
        {
            if (_context.Vehicles.Any(p => p.PlateNo.Equals(request.PlateNo.ToUpper())))
            {
                throw new AlreadyExistsException(nameof(Vehicle), nameof(request.PlateNo), request.PlateNo);
            }
            var vehicle = new Vehicle()
            {
                PlateNo = request.PlateNo,
                CompanyId = request.CompanyId.ToInt()
            };
            _context.Add(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new VehicleInfoResponseModel()
            {
                Id = vehicle.Id,
                PlateNo = vehicle.PlateNo,
                CompanyId = (CompanyType)vehicle.CompanyId,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(vehicle.CreatedDate),
            });
        }
    }

}