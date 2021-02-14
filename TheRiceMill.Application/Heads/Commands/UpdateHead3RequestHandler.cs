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
    public class UpdateHead3RequestHandler : IRequestHandler<UpdateHead3RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateHead3RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateHead3RequestModel request, CancellationToken cancellationToken)
        {
            var head3 = _context.Head3.GetBy(p => p.Id == request.Id);
            if (head3 == null)
            {
                throw new NotFoundException(nameof(head3), request.Id);
            }

            //head3.Code = request.Code;
            //head3.CompanyId = request.CompanyId.ToInt();
            head3.Name = request.Name;
            head3.Type = request.Type;

            _context.Head3.Update(head3);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head3);
        }
    }
}
