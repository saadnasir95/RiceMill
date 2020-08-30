using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Products.Models
{
    public class GetProductsForComboBoxRequestModel : IRequest<ResponseViewModel>
    {
        public void SetDefaultValue()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = "";
            }
            
        }
        public string Search { get; set; }
        public bool IsProcessedMaterial { get; set; }
        public int PageSize { get; set; }
        public CompanyType CompanyId { get; set; }
    }


}