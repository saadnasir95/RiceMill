using FluentValidation;
using TheRiceMill.Application.Extensions;

namespace TheRiceMill.Application.Bank.Queries.GetAllBanks
{
    public class GetAllBanksRequestModelValidator : AbstractValidator<GetAllBanksRequestModel>
    {
        public GetAllBanksRequestModelValidator()
        {
        }
    }
}