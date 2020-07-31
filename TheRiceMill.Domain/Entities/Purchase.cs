using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Purchase : Base
    {
        public DateTime Date { get; set; }
        public double TotalMaund { get; set; }
        public double RatePerMaund { get; set; }
        /// <summary>
        /// Total Price of Invoice
        /// </summary>
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public List<Charge> Charges { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }
}