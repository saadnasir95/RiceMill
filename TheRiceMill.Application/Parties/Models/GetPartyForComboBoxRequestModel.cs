using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class GetPartyForComboBoxRequestModel : IRequest<ResponseViewModel>
    {
        public GetPartyForComboBoxRequestModel()
        {
            Search = "";
        }
        public string Search { get; set; }
        public int PageSize { get; set; }
        public CompanyType CompanyId { get; set; }
    }
}