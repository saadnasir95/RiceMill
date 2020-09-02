using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Lot : Base
    {
        public int Year { get; set; }
        public List<StockIn> StockIns { get; set; }
        public List<StockOut> StockOuts { get; set; }
        public List<ProcessedMaterial> ProcessedMaterials { get; set; }
        public List<RateCost> RateCosts { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }
}
