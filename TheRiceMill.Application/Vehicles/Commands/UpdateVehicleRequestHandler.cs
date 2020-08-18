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

    public class UpdateVehicleRequestHandler : IRequestHandler<UpdateVehicleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateVehicleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateVehicleRequestModel request, CancellationToken cancellationToken)
        {
            var vehicle = await _context.Vehicles.FindAsync(request.Id);
            if (vehicle == null)
            {
                throw new NotFoundException(nameof(Vehicle), request.Id);
            }

            if (!vehicle.PlateNo.ToUpper().Equals(request.PlateNo.ToUpper()))
            {
                if (_context.Vehicles.Any(p => p.PlateNo.ToUpper().Equals(request.PlateNo.ToUpper())))
                {
                    throw new AlreadyExistsException(nameof(Vehicle), nameof(request.PlateNo), request.PlateNo);
                }
            }

            vehicle.PlateNo = request.PlateNo.ToUpper();
            vehicle.CompanyId = request.CompanyId.ToInt();
            _context.Update(vehicle);
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