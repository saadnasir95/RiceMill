using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Sale : Base
    {
        public DateTime Date { get; set; }
        public double TotalMaund { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }

        public double Rate { get; set; }
        public RateBasedOn RateBasedOn { get; set; }

        /// <summary>
        /// Maund/Bag * Rate        
        /// </summary>
        public double BasePrice { get; set; }

        /// <summary>
        /// Total Price of Invoice
        /// </summary>
        public double TotalPrice { get; set; }
        public double Freight { get; set; }

        public double Commission { get; set; }
        public List<Charge> Charges { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }
}