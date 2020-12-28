using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Heads.Commands
{
    public class DeleteHead1RequestHandler : IRequestHandler<DeleteHead1RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteHead1RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteHead1RequestModel request, CancellationToken cancellationToken)
        {
            var head1 = _context.Head1.GetBy(p => p.Id == request.Id);
            if (head1 == null)
            {
                throw new NotFoundException(nameof(head1), request.Id);
            }
            _context.Head1.Remove(head1);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(true);
        }
    }
}
