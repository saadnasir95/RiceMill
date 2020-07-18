namespace TheRiceMill.Domain.Entities
{
    public class Charge : Base
    {
        public int? SaleId { get; set; }
        public Sale Sale { get; set; }
        public int? PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
        public string Task { get; set; }
        public int BagQuantity { get; set; }
        public double Rate { get; set; }
        public double Total { get; set; }
        public bool AddPrice { get; set; }
    }
}