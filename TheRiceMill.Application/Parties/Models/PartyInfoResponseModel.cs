using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.Companies.Models
{
    public class PartyInfoResponseModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }
        public CompanyType CompanyId { get; set; }
        public string CreatedDate { get; set; }
    }
}