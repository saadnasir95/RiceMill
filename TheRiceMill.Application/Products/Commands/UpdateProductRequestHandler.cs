using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Products.Commands
{
    public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateProductRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateProductRequestModel request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                throw new NotFoundException(nameof(Product),request.Id);
            }

            if (!product.NormalizedName.Equals(request.Name.ToUpper()))
            {
                if (_context.Products.Any(p => p.NormalizedName.Equals(request.Name.ToUpper())))
                {
                    throw new AlreadyExistsException(nameof(Product), nameof(request.Name), request.Name);
                }
            }

            product.Name = request.Name;
            product.IsProcessedMaterial = request.IsProcessedMaterial;
            product.NormalizedName = request.Name.ToUpper();
            product.CompanyId = request.CompanyId.ToInt();
            _context.Products.Update(product);
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