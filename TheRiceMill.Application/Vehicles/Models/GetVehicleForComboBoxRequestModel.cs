using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Vehicles.Models
{
    public class GetVehicleForComboBoxRequestModel : IRequest<ResponseViewModel>
    {
        public void SetDefaultValue()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = "";
            }
        }
        public string Search { get; set; }
        public int PageSize { get; set; }
        public CompanyType CompanyId { get; set; }
    }

}