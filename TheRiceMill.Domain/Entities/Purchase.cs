using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Purchase : Base
    {
        public DateTime Date { get; set; }
        public double TotalMaund { get; set; }
        public double BoriQuantity { get; set; }

        public double RatePerMaund { get; set; }
        public RateBasedOn RateBasedOn { get; set; }

        /// <summary>
        /// Total Price of Invoice
        /// </summary>
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public List<Charge> Charges { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }

    public enum RateBasedOn
    {
        Maund = 1,
        Bori = 2,
        Bag = 3
    }
}