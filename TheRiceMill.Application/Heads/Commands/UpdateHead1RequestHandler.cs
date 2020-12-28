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
    public class UpdateHead1RequestHandler : IRequestHandler<UpdateHead1RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateHead1RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateHead1RequestModel request, CancellationToken cancellationToken)
        {
            var head1 = _context.Head1.GetBy(p => p.Id == request.Id);
            if (head1 == null)
            {
                throw new NotFoundException(nameof(head1), request.Id);
            }

            head1.Code = request.Code;
            head1.CompanyId = request.CompanyId.ToInt() ;
            head1.Name = request.Name;
            head1.Type = request.HeadType.ToInt();

            _context.Head1.Update(head1);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head1);
        }
    }
}
