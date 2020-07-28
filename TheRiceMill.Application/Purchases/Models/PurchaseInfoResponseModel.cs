namespace TheRiceMill.Application.Purchases.Models
{
    public class PurchaseInfoResponseModel
    {
        public string CreatedDate { get; set; }
        public string Party { get; set; }
        public int BagQuantity { get; set; }
        /// <summary>
        /// Expected Weight of Bags (60-65)
        /// </summary>
        public double ExpectedBagWeight { get; set; }
        /// <summary>
        /// Total Expected Weight of Bags = Quantity of Bags x Expected Bag Weight
        /// </summary>
        public double TotalExpectedBagWeight { get; set; }
        /// <summary>
        /// The Weight Measured by the Guys by Machine
        /// </summary>
        public double KandaWeight { get; set; }
        /// <summary>
        /// Expected Weight of Empty Bags (1-2)
        /// </summary>
        public double ExpectedEmptyBagWeight { get; set; }

        /// <summary>
        /// Total Expected Weight of Empty Bags = Quantity of Bags x Expected Empty Bag Weight
        /// </summary>
        public double TotalExpectedEmptyBagWeight { get; set; }
        /// <summary>
        /// Vibration = Quantity of Bags x 0.2
        /// </summary>
        public double Vibration { get; set; }
        /// <summary>
        /// Total Actual Weight of Bags = KandaWeight - TotalExpectedEmptyBagWeight - Vibration
        /// </summary>
        public double TotalActualBagWeight { get; set; }
        /// <summary>
        /// Actual Bag Weight = TotalActualBagWeight / Quantity of Bags
        /// </summary>
        public double ActualBagWeight { get; set; }
        /// <summary>
        /// Total Maund = Total Actual Weight of Bags / 40
        /// </summary>
        public double TotalMaund { get; set; }
        /// <summary>
        /// Navigation Property for Product
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// Rate of Product PerKg
        /// </summary>
        public double RatePerKg { get; set; }
        /// <summary>
        /// Rate of Product Per Maund
        /// </summary>
        public double RatePerMaund { get; set; }
        /// <summary>
        /// Total Price of Product
        /// </summary>
        public double TotalPrice { get; set; }

    }
}