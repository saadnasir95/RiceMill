using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Lots.Queries
{
    class GetLotRequestHandler : IRequestHandler<GetLotRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetLotRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetLotRequestModel request, CancellationToken cancellationToken)
        {
            object lots = null;
            request.SetDefaultValue();
            if (request.LotId != 0) {
                lots = _context.Lots.GetMany(q => q.Id == request.LotId, request.OrderBy, request.Page,
                request.PageSize, request.IsDescending, p => p.Include(pr => pr.StockIns).Include(pt => pt.StockOuts)
                .Include(py => py.ProcessedMaterials).Include(pu => pu.RateCosts).Include((pi => pi.GatePasses))).Select(p => new GetLotResponseModel()
                {
                    GatePasses = p.GatePasses,
                    RateCosts = p.RateCosts,
                    ProcessedMaterials = this.ProcessedMaterialMapper(p.ProcessedMaterials),
                    StockIns = p.StockIns,
                    StockOuts = p.StockOuts,
                    Year = p.Year
                }).ToList();

            }
            return new ResponseViewModel().CreateOk(lots);
        }


        private List<Models.ProcessedMaterial> ProcessedMaterialMapper(List<Domain.Entities.ProcessedMaterial> processedMaterial)
        {
            List<Models.ProcessedMaterial> _processedMaterial = new List<Models.ProcessedMaterial>();
            processedMaterial.ForEach(pm =>
            {
                Models.ProcessedMaterial _pm = new Models.ProcessedMaterial();
                _pm.BagQuantity = pm.BagQuantity;
                _pm.BoriQuantity = pm.BoriQuantity;
                _pm.PerKG = pm.PerKG;
                _pm.TotalKG = pm.TotalKG;
                _pm.Product = pm.Product;
                _pm.ProductId = pm.ProductId;
                _pm.Id = pm.Id;
                _processedMaterial.Add(_pm);

            });
            return _processedMaterial;
        }
    }
}
