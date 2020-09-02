using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    /// <summary>
    /// Product Entity
    /// </summary>
    public class Product : Base
    {
        /// <summary>
        /// Name of the Product e.g "Apple"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NormalizedName of the Product e.g "APPLE"
        /// </summary>
        public string NormalizedName { get; set; }
        public bool IsProcessedMaterial { get; set; }
        public IList<GatePass> GatePasses { get; set; }
        public IList<ProcessedMaterial> ProcessedMaterials { get; set; }
        public IList<StockOut> StockOuts { get; set; }
    }
}