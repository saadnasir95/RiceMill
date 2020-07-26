using System;
using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class GatePass : Base
    {
        public DateTime DateTime { get; set; }
        public int Type { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? SaleId { get; set; }
        public Sale Sale { get; set; }
        public int? PurchaseId { get; set; }
        public Purchase Purhcase { get; set; }
        public string Broker { get; set; }
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }
        public double WeightPerBag { get; set; }
        public double NetWeight { get; set; }
        public double Maund { get; set; }
    }
}