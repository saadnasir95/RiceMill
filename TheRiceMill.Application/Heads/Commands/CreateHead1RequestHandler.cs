using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

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
            Expression<Func<Head1, bool>> query = p => p.CompanyId == request.CompanyId.ToInt();
            string code = "01-00-00-00-0000";
            var head1Codes = _context.Head1
                .GetMany(query, "Id", 1, 1, true)
                .Select(c => c.Code).ToList();
            if (head1Codes?.Count > 0)
            {
                string[] codeArray = head1Codes[0].Split('-');
                codeArray[0] = (Convert.ToInt32(codeArray[0]) + 1).ToString().PadLeft(2, '0');
                code = String.Join("-", codeArray);
            }
            Head1 head = new Head1()
            {
                Code = code,
                CompanyId = request.CompanyId.ToInt(),
                Name = request.Name,
                Type = request.Type,
            };

            _context.Head1.Add(head);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(head);
        }
    }
}
