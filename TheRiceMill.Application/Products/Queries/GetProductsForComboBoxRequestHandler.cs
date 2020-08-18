using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Products.Queries
{
    public class GetProductsForComboBoxRequestHandler : IRequestHandler<GetProductsForComboBoxRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetProductsForComboBoxRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetProductsForComboBoxRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = _context.Products.GetMany(p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt(), p => p.Name, 1,
                request.PageSize, true).Select(product => new ProductsInfoComboBoxResponseModel()
                {
                    Name = product.Name,
                    Id = product.Id,
                    CompanyId = (CompanyType)product.CompanyId,
                }).ToList();
            return Task.FromResult(new ResponseViewModel().CreateOk(list));
        }
    }
}