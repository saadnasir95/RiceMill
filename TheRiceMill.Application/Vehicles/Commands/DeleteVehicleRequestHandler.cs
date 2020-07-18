using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Vehicles.Commands
{

    public class DeleteVehicleRequestHandler : IRequestHandler<DeleteVehicleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteVehicleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(DeleteVehicleRequestModel request, CancellationToken cancellationToken)
        {
            var canDelete = _context.Vehicles.Any(p => p.Id == request.Id && !p.GatePasses.Any() && !p.Sales.Any());
            if (!canDelete)
            {
                throw new CannotDeleteException(nameof(Vehicle), request.Id);
            }

            _context.Vehicles.Remove(new Vehicle()
            {
                Id = request.Id
            });
            _context.SaveChanges();
            return Task.FromResult(new ResponseViewModel().CreateOk());
        }
    }

}