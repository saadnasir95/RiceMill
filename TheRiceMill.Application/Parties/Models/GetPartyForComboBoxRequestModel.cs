using MediatR;
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
    }
}