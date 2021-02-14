using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Heads.Commands
{
    public class UpdateHead2RequestHandler : IRequestHandler<UpdateHead2RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateHead2RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateHead2RequestModel request, CancellationToken cancellationToken)
        {
            var head2 = _context.Head2.GetBy(p => p.Id == request.Id);
            if (head2 == null)
            {
                throw new NotFoundException(nameof(head2), request.Id);
            }

            //head1.Code = request.Code;
            //head1.CompanyId = request.CompanyId.ToInt();
            head2.Name = request.Name;
            head2.Type = request.Type;

            _context.Head2.Update(head2);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head2);
        }
    }
}
