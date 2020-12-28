using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Heads.Commands
{
    class CreateHead1RequestHandler : IRequestHandler<CreateHead1RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateHead1RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateHead1RequestModel request,
            CancellationToken cancellationToken)
        {
            Head1 head = new Head1()
            {
                Code = request.Code,
                CompanyId = request.CompanyId.ToInt(),
                Name = request.Name,
                Type = request.HeadType.ToInt(),
            };

            _context.Head1.Add(head);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head);
        }
    }
}
