using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Lots.Commands
{
    class CreateProcessedMaterialRequestHandler : IRequestHandler<CreateProcessedMaterialRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateProcessedMaterialRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateProcessedMaterialRequestModel request,
            CancellationToken cancellationToken)
        {
            var _processedMaterial = new Domain.Entities.ProcessedMaterial();
            List<Models.ProcessedMaterial> _processedMaterials = new List<Models.ProcessedMaterial>();
            request.ProcessedMaterials.ForEach(async pm =>
            {
                var processedMaterial = new Domain.Entities.ProcessedMaterial();
                processedMaterial.BagQuantity = pm.BagQuantity;
                processedMaterial.BoriQuantity = pm.BoriQuantity;
                processedMaterial.PerKG = pm.PerKG;
                processedMaterial.ProductId = pm.ProductId;
                processedMaterial.TotalKG = pm.TotalKG;
                processedMaterial.LotId = request.LotId;
                processedMaterial.LotYear = request.LotYear;
                _processedMaterial = processedMaterial;
                _context.ProcessedMaterials.Add(processedMaterial);
                //await _context.SaveChangesAsync();
            });
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new ProcessedMaterialResponseModel()
            {
              LotId = _processedMaterial.LotId,
              Id = _processedMaterial.Id,
              Lot = _processedMaterial.Lot,
              LotYear = _processedMaterial.LotYear,
            });
        }
    }
}
