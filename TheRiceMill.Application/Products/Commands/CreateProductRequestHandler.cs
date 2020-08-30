using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Application.Users.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Products.Commands
{
    public class CreateProductRequestHandler : IRequestHandler<CreateProductRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateProductRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateProductRequestModel request, CancellationToken cancellationToken)
        {
            if (_context.Products.Any(p => p.NormalizedName.Equals(request.Name.ToUpper())))
            {
                throw new AlreadyExistsException(nameof(Product), nameof(request.Name), request.Name);
            }
            var product = new Product()
            {
                Name = request.Name,
                IsProcessedMaterial = request.IsProcessedMaterial,
                NormalizedName = request.Name.ToUpper(),
                CompanyId = request.CompanyId.ToInt()
            };
            _context.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new ProductInfoResponseModel()
            {
                Name = product.Name,
                Id = product.Id,
                IsProcessedMaterial = product.IsProcessedMaterial,
                CompanyId = (CompanyType)product.CompanyId,
                CreatedDate = new DateConverter().ConvertToDateTimeIso(product.CreatedDate),
            });
        }
    }
}