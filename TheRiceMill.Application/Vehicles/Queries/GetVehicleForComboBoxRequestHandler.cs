using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Vehicles.Queries
{

    public class GetVehicleForComboBoxRequestHandler : IRequestHandler<GetVehicleForComboBoxRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetVehicleForComboBoxRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetVehicleForComboBoxRequestModel request,
            CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = _context.Vehicles.GetMany(
                p => p.PlateNo.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt(), p => p.PlateNo,
                1,
                request.PageSize, true).Select(vehicle => new VehicleComboBoxInfoResponseModel()
                {
                    Id = vehicle.Id,
                    PlateNo = vehicle.PlateNo,
                    CompanyId = (CompanyType)vehicle.CompanyId
                }).ToList();
            return Task.FromResult(new ResponseViewModel().CreateOk(list));
        }
    }

}