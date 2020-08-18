using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Vehicle : Base
    {
        public string PlateNo { get; set; }
        public IEnumerable<GatePass> GatePasses { get; set; }
    }
}