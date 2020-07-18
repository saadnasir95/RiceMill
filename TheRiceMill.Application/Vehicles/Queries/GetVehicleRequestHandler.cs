using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Vehicles.Queries
{

    public class GetVehicleRequestHandler : IRequestHandler<GetVehicleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetVehicleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetVehicleRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            List<VehicleInfoResponseModel> list = _context.Vehicles.GetMany(
                p => p.Name.Contains(request.Search) || p.PlateNo.Contains(request.Search), request.OrderBy,
                request.Page,
                request.PageSize, request.IsDescending).Select(vehicle => new VehicleInfoResponseModel()
            {
                Name = vehicle.Name,
                Id = vehicle.Id,
                PlateNo = vehicle.PlateNo,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(vehicle.CreatedDate),
            }).ToList();
            var count = _context.Vehicles.Count(p => p.Name.Contains(request.Search) || p.PlateNo.Contains(request.Search));
            return Task.FromResult(new ResponseViewModel().CreateOk(list, count));
        }
    }

}