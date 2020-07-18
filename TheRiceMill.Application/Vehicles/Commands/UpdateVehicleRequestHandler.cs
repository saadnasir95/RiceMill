using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Application.Vehicles.Models;
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
                    throw new AlreadyExistsException(nameof(Vehicle), nameof(request.Name), request.Name);
                }
            }

            vehicle.Name = request.Name;
            vehicle.NormalizedName = request.Name.ToUpper();
            vehicle.PlateNo = request.PlateNo.ToUpper();
            _context.Update(vehicle);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new VehicleInfoResponseModel()
            {
                Name = vehicle.Name,
                Id = vehicle.Id,
                PlateNo = vehicle.PlateNo,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(vehicle.CreatedDate),
            });
        }
    }

}