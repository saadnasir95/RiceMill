using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Products.Queries
{
    public class GetProductsRequestHandler : IRequestHandler<GetProductsRequestModel,ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetProductsRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetProductsRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var list = _context.Products.GetMany(p => p.Name.Contains(request.Search), request.OrderBy, request.Page,
                request.PageSize, request.IsDescending).Select(product => new ProductInfoResponseModel()
            {
                Name = product.Name,
                Id = product.Id,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(product.CreatedDate),
            }).ToList();
            var count = _context.Products.Count(p => p.Name.Contains(request.Search));
            return Task.FromResult(new ResponseViewModel().CreateOk(list,count));
        }
    }
}