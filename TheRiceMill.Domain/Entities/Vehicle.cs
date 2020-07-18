using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Vehicle : Base
    {
        /// <summary>
        /// Name of the Vehicle e.g "Apple"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NormalizedName of the Vehicle e.g "APPLE"
        /// </summary>
        public string NormalizedName { get; set; }

        public string PlateNo { get; set; }
        public IEnumerable<GatePass> GatePasses { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}