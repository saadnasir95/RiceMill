using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Sale : Base
    {
        public DateTime CheckOut { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double BagQuantity { get; set; }
        public double BagWeight { get; set; }
        public double ExpectedBagWeight { get; set; }
        public double TotalExpectedBagWeight { get; set; }
        public double KandaWeight { get; set; }
        public double ExpectedEmptyBagWeight { get; set; }
        public double TotalExpectedEmptyBagWeight { get; set; }
        public double ActualBagWeight { get; set; }
        public double TotalActualBagWeight { get; set; }
        public double TotalMaund { get; set; }
        public double RatePerKg { get; set; }
        public double RatePerMaund { get; set; }
        public double BasePrice { get; set; }
        public double TotalPrice { get; set; }
        public string BiltyNumber { get; set; }
        public double Commission { get; set; }
        public double PercentCommission { get; set; }
        public List<Charge> Charges { get; set; }
        public List<GatePass> GatePasses { get; set; }
    }
}