namespace TheRiceMill.Application.Products.Models
{
    public class ProductInfoResponseModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Type { get; set; }
        public int Id { get; set; }
        public string CreatedDate { get; set; }
    }

    public class ProductsInfoComboBoxResponseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public double Price { get; set; }
    }
}