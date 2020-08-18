using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.Companies.Models
{
    public class PartyInfoComboBoxResponseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public CompanyType CompanyId { get; set; }
    }
}