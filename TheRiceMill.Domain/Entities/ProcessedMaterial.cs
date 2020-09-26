using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class ProcessedMaterial : Base
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public Lot Lot { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double PerKG { get; set; }
        public double TotalKG { get; set; }
    }
}
