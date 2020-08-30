using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class GetProductsRequestHandler : IRequestHandler<GetProductsRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetProductsRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetProductsRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Domain.Entities.Product, bool>> query = CreateQuery(request);
            var list = _context.Products.GetMany(query, request.OrderBy, request.Page,
                request.PageSize, request.IsDescending).Select(product => new ProductInfoResponseModel()
                {
                    Name = product.Name,
                    Id = product.Id,
                    IsProcessedMaterial = product.IsProcessedMaterial,
                    CompanyId = (CompanyType)product.CompanyId,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(product.CreatedDate),
                }).ToList();
            var count = _context.Products.Count(query);
            return Task.FromResult(new ResponseViewModel().CreateOk(list, count));
        }

        private Expression<Func<Domain.Entities.Product, bool>> CreateQuery(GetProductsRequestModel request)
        {
            if (request.ProductType == (int)ProductType.All)
            {
                return p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt();
            }
            if (request.ProductType == (int)ProductType.ProcessedMaterial)
            {
                return p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt() && p.IsProcessedMaterial == true;
            }
            if (request.ProductType == (int)ProductType.NonProcessedMaterial)
            {
                return p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt() && p.IsProcessedMaterial == false;
            }
            else
            {
                return p => p.Name.Contains(request.Search) && p.CompanyId == request.CompanyId.ToInt();
            }
        }
    }
}