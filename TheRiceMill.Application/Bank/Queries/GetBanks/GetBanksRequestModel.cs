using MediatR;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Bank.Queries.GetBanks
{

    public class GetBanksRequestModel : IRequest<ResponseViewModel>
    {
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool IsDescending { get; set; }


        public void SetDefaultValue()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Search = "";
            }
            if (string.IsNullOrEmpty(OrderBy))
            {
                OrderBy = "CreatedDate";
            }
        }
    }
}