namespace TheRiceMill.Application.Vehicles.Models
{
    public class VehicleInfoResponseModel
    {
        public string Name { get; set; }
        public string PlateNo { get; set; }
        public string CreatedDate { get; set; }
        public int Id { get; set; }
    }

    public class VehicleComboBoxInfoResponseModel
    {
        public string Name { get; set; }
        public string PlateNo { get; set; }
        public int Id { get; set; }

    }
}