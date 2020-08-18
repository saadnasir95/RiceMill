using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.Vehicles.Models
{
    public class VehicleInfoResponseModel
    {
        public string PlateNo { get; set; }
        public string CreatedDate { get; set; }
        public int Id { get; set; }
        public CompanyType CompanyId { get; set; }
    }

    public class VehicleComboBoxInfoResponseModel
    {
        public CompanyType CompanyId { get; set; }
        public string PlateNo { get; set; }
        public int Id { get; set; }

    }
}