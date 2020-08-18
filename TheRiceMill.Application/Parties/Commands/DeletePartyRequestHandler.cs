using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheRiceMill.Application.Companies.Models;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Companies.Commands
{
    public class DeletePartyRequestHandler : IRequestHandler<DeletePartyRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeletePartyRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(DeletePartyRequestModel request, CancellationToken cancellationToken)
        {
            var canDelete = _context.Parties.Any(p => p.Id == request.Id && !p.GatePasses.Any() && !p.Ledgers.Any());
            if (!canDelete)
            {
                throw new CannotDeleteException(nameof(Party), request.Id);
            }

            _context.Parties.Remove(new Party()
            {
                Id = request.Id
            });
            _context.SaveChanges();
            return Task.FromResult(new ResponseViewModel().CreateOk());
        }
    }
}