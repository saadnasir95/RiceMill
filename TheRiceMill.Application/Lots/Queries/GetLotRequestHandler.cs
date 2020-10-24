using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
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
            Lot lot = null;
            request.SetDefaultValue();
            if (request.LotId != 0)
            {
                lot = _context.Lots.GetBy(q => q.Id == request.LotId && q.Year == request.LotYear && q.CompanyId == request.CompanyId.ToInt(),
                p => p.Include(pr => pr.StockIns)
                .Include(pt => pt.StockOuts).ThenInclude(c => c.Product)
                .Include(py => py.ProcessedMaterials).ThenInclude(c => c.Product)
                .Include(pu => pu.RateCosts)
                .Include(pi => pi.GatePasses).ThenInclude(sa => sa.Purhcase)
                .Include(pi => pi.GatePasses).ThenInclude(inv => inv.Sale)
                .Include(pi => pi.GatePasses).ThenInclude(pa => pa.Party)
                .Include(pi => pi.GatePasses).ThenInclude(pa => pa.Vehicle));
            }
            if (lot != null)
            {
                return new ResponseViewModel().CreateOk(new GetLotResponseModel
                {
                    RateCosts = this.RateCostMapper(lot.RateCosts),
                    ProcessedMaterials = this.ProcessedMaterialMapper(lot.ProcessedMaterials),
                    StockIns = this.StockInMapper(lot.StockIns),
                    StockOuts = this.StockOutMapper(lot.StockOuts),
                    Year = lot.Year,
                    Id = lot.Id,
                    Sales = new GatepassMapper().MapGatepassToLotSale(lot.GatePasses),
                    Purchases = new GatepassMapper().MapGatepassToLotPurchase(lot.GatePasses)
            });
            }
            else
            {
                return new ResponseViewModel().CreateOk(new { });
            }
        }


        private List<ProcessedMaterialRequest> ProcessedMaterialMapper(List<ProcessedMaterial> processedMaterial)
        {
            List<ProcessedMaterialRequest> _processedMaterial = new List<ProcessedMaterialRequest>();
            processedMaterial.ForEach(pm =>
            {
                ProcessedMaterialRequest _pm = new ProcessedMaterialRequest();
                _pm.BagQuantity = pm.BagQuantity;
                _pm.BoriQuantity = pm.BoriQuantity;
                _pm.PerKG = pm.PerKG;
                _pm.TotalKG = pm.TotalKG;
                _pm.Product = new ProductRequestModel
                {
                    Name = pm.Product.Name
                };
                _pm.ProductId = pm.ProductId;
                _pm.Id = pm.Id;
                _processedMaterial.Add(_pm);

            });
            return _processedMaterial;
        }
        private List<StockInRequestModel> StockInMapper(List<StockIn> stockIns)
        {
            List<StockInRequestModel> _stockInRequestList = new List<StockInRequestModel>();
            stockIns?.ForEach(stockIn =>
            {
                StockInRequestModel _stockIn = new StockInRequestModel()
                {
                    BagQuantity = stockIn.BagQuantity,
                    BoriQuantity = stockIn.BoriQuantity,
                    TotalKG = stockIn.TotalKG,
                    Id = stockIn.Id,
                    LotId = stockIn.LotId,
                    LotYear = stockIn.LotYear,
                    GatepassTime = stockIn.GatepassTime
                };
                _stockInRequestList.Add(_stockIn);

            });
            return _stockInRequestList;
        }
        private List<StockOutRequestModel> StockOutMapper(List<StockOut> stockOuts)
        {
            List<StockOutRequestModel> _stockOutRequestList = new List<StockOutRequestModel>();
            stockOuts?.ForEach(stockOut =>
            {
                StockOutRequestModel _stockOut = new StockOutRequestModel()
                {
                    BagQuantity = stockOut.BagQuantity,
                    BoriQuantity = stockOut.BoriQuantity,
                    TotalKG = stockOut.TotalKG,
                    Id = stockOut.Id,
                    LotId = stockOut.LotId,
                    LotYear = stockOut.LotYear,
                    PerKG = stockOut.PerKG,
                    Product = new ProductRequestModel
                    {
                        Name = stockOut.Product.Name
                    },
                    ProductId = stockOut.ProductId
                };
                _stockOutRequestList.Add(_stockOut);
            });
            return _stockOutRequestList;
        }
        private List<RateCostRequestModel> RateCostMapper(List<RateCost> rateCosts)
        {
            List<RateCostRequestModel> rateCostList = new List<RateCostRequestModel>();
            rateCosts?.ForEach(rateCost =>
            {
                RateCostRequestModel _stockIn = new RateCostRequestModel()
                {
                    Id = rateCost.Id,
                    LotId = rateCost.LotId,
                    LotYear = rateCost.LotYear,
                    BardanaAndMisc = rateCost.BardanaAndMisc,
                    Freight = rateCost.Freight,
                    GrandTotal = rateCost.Freight,
                    LabourUnloadingAndLoading = rateCost.LabourUnloadingAndLoading,
                    ProcessingExpense = rateCost.ProcessingExpense,
                    PurchaseBrokery = rateCost.PurchaseBrokery,
                    RatePer40LessByProduct = rateCost.RatePer40LessByProduct,
                    RatePer40WithoutProcessing = rateCost.RatePer40WithoutProcessing,
                    SaleBrokery = rateCost.SaleBrockery,
                    Total = rateCost.Total
                };
                rateCostList.Add(_stockIn);
            });
            return rateCostList;
        }
    }
}
