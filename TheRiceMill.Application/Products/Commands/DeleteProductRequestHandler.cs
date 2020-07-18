using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Products.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Products.Commands
{
    public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteProductRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(DeleteProductRequestModel request, CancellationToken cancellationToken)
        {
            var canDelete = _context.Products.Any(p => p.Id == request.Id && !p.Purchases.Any() && !p.Sales.Any());
            if (!canDelete)
            {
                throw new CannotDeleteException(nameof(Product), request.Id);
            }

            _context.Products.Remove(new Product()
            {
                Id = request.Id
            });
            _context.SaveChanges();
            return Task.FromResult(new ResponseViewModel().CreateOk());
        }
    }
}