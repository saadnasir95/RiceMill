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
    public class UpdateHead5RequestHandler : IRequestHandler<UpdateHead5RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateHead5RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateHead5RequestModel request, CancellationToken cancellationToken)
        {
            var head5 = _context.Head5.GetBy(p => p.Id == request.Id);
            if (head5 == null)
            {
                throw new NotFoundException(nameof(head5), request.Id);
            }

            //head5.Code = request.Code;
            //head5.CompanyId = request.CompanyId.ToInt();
            head5.Name = request.Name;
            head5.Type = request.Type;

            _context.Head5.Update(head5);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head5);
        }
    }
}
