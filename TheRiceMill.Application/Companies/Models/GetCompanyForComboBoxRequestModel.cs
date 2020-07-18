using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Companies.Models
{
    public class GetCompanyForComboBoxRequestModel : IRequest<ResponseViewModel>
    {
        public GetCompanyForComboBoxRequestModel()
        {
            Search = "";
        }
        public string Search { get; set; }
        public int PageSize { get; set; }
    }
}