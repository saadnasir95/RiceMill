using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Lots.Commands
{
    public class UpdateProcessedMaterialRequestHandler : IRequestHandler<UpdateProcessedMaterialRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateProcessedMaterialRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseViewModel> Handle(UpdateProcessedMaterialRequestModel request,
            CancellationToken cancellationToken)
        {
            var processedMaterialsListInDB = _context.ProcessedMaterials.GetMany(c => c.LotId == request.LotId && c.LotYear == request.LotYear && c.CompanyId == request.CompanyId.ToInt(), "Id", 1, 20).ToList();
            if (processedMaterialsListInDB != null && processedMaterialsListInDB.Count > 0)
            {
                foreach (var processedMaterial in processedMaterialsListInDB)
                {
                    var requestProcessedMaterial = request.ProcessedMaterials.FirstOrDefault(c => c.ProductId == processedMaterial.ProductId);
                    if (requestProcessedMaterial != null)
                    {
                        processedMaterial.PerKG = requestProcessedMaterial.PerKG;
                        processedMaterial.TotalKG = requestProcessedMaterial.TotalKG;
                        processedMaterial.BoriQuantity = requestProcessedMaterial.BoriQuantity;
                        processedMaterial.BagQuantity = requestProcessedMaterial.BagQuantity;
                    }
                    else
                    {
                        processedMaterialsListInDB.Remove(processedMaterial);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NotFoundException(nameof(ProcessedMaterial), request.ProcessedMaterials);
            }
            return new ResponseViewModel().CreateOk(new ProcessedMaterialResponseModel()
            {
                LotId = request.LotId,
                LotYear = request.LotYear,
                CompanyId = request.CompanyId
            });
        }
    }
}
