using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Sales.Queries.GetSales
{

    public class GetSalesRequestModel : IRequest<ResponseViewModel>
    {
        public void SetDefaultValue()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = "";
            }

            if (string.IsNullOrWhiteSpace(OrderBy))
            {
                OrderBy = "CreatedDate";
            }
        }
        public string Search { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public CompanyType CompanyId { get; set; }
    }
}