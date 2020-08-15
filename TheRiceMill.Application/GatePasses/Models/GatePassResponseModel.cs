using System;
using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class GatePassResponseModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The Type of the GatePass 1 = OutwardGatePass and 2 = InwardGatePass
        /// </summary>
        public GatePassType Type { get; set; }
        public int PartyId { get; set; }
        public PartyRequestModel Party { get; set; }
        public int VehicleId { get; set; }
        public VehicleRequestModel Vehicle { get; set; }
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }
        /// <summary>
        /// Total Expected Weight of Bags = Quantity of Bags x Expected Bag Weight
        /// </summary>
        public double WeightPerBag { get; set; }
        /// <summary>
        /// The Weight Measured by the Guys by Machine
        /// </summary>
        public double KandaWeight { get; set; }
        public double EmptyWeight { get; set; }
        public double NetWeight { get; set; }
        /// <summary>
        /// Total Maund = Total Actual Weight of Bags / 40
        /// </summary>
        public double Maund { get; set; }
        public DateTime DateTime { get; set; }
        public string Broker { get; set; }
        public int? PurchaseId { get; set; }
        public int? SaleId { get; set; }
        public string BiltyNumber { get; set; }
        public string LotNumber { get; set; }
    }
}