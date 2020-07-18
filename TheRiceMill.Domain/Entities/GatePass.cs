using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class GatePass : Base
    {
        public DateTime? CheckIn { get; set; }
        /// <summary>
        /// The Type of the GatePass 1 = Sale and 2 = Purchase
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// The Direction of the GatePass 1. Milling/2. Stockpile/3. Outside
        /// </summary>
        public int Direction { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public string BiltyNumber { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double BagQuantity { get; set; }
        public double BagWeight { get; set; }
        public double KandaWeight { get; set; }
        public double TotalMaund { get; set; }
    }
}