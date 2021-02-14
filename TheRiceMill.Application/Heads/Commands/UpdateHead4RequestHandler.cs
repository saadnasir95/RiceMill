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
    public class UpdateHead4RequestHandler : IRequestHandler<UpdateHead4RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateHead4RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateHead4RequestModel request, CancellationToken cancellationToken)
        {
            var head4 = _context.Head4.GetBy(p => p.Id == request.Id);
            if (head4 == null)
            {
                throw new NotFoundException(nameof(head4), request.Id);
            }

            //head4.Code = request.Code;
            //head4.CompanyId = request.CompanyId.ToInt();
            head4.Name = request.Name;
            head4.Type = request.Type;

            _context.Head4.Update(head4);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head4);
        }
    }
}
