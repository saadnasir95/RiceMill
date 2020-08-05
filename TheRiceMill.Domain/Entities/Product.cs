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

        public IEnumerable<GatePass> GatePasses { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}