using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Lots.Models
{
    public class GetLotResponseModel
    {
        public int Year { get; set; }
        public List<StockIn> StockIns { get; set; }
        public List<StockOut> StockOuts { get; set; }
        public List<ProcessedMaterial> ProcessedMaterials { get; set; }
        public List<RateCost> RateCosts { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }
}
