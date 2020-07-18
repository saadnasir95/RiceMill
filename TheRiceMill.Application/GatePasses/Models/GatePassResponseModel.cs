using System;
using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class GatePassResponseModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The Type of the GatePass 1 = GetOut and 2 = GetIn
        /// </summary>
        public GatePassType Type { get; set; }

        /// <summary>
        /// The Direction of the GatePass 1. Milling/2. Stockpile/3. Outside
        /// </summary>
        public Direction Direction { get; set; }
        public int CompanyId { get; set; }
        public CompanyRequestModel Company { get; set; }
        public int VehicleId { get; set; }
        public VehicleRequestModel Vehicle { get; set; }
        public string BiltyNumber { get; set; }
        /// <summary>
        /// Quantity of Bags
        /// </summary>
        public double BagQuantity { get; set; }
        /// <summary>
        /// Total Expected Weight of Bags = Quantity of Bags x Expected Bag Weight
        /// </summary>
        public double BagWeight { get; set; }
        /// <summary>
        /// The Weight Measured by the Guys by Machine
        /// </summary>
        public double KandaWeight { get; set; }
        /// <summary>
        /// Total Maund = Total Actual Weight of Bags / 40
        /// </summary>
        public double TotalMaund { get; set; }
        /// <summary>
        /// ProductId of Product
        /// </summary>
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public DateTime CheckDateTime { get; set; }
    }
}