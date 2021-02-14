using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Heads.Commands
{
    class CreateHead2RequestHandler : IRequestHandler<CreateHead2RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateHead2RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateHead2RequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Head1, bool>> query = p => p.Id == request.Head1Id;
            var head1List = _context.Head1
                .GetMany(query, "Id", 1, 1, true)
                .Select(head1 => new Head1
                {
                    Id = head1.Id,
                    Code = head1.Code,
                    Type = head1.Type,
                    Name = head1.Name,
                    Head2 = head1.Head2.Select(head2 => new Head2
                    {
                        Id = head2.Id,
                        Code = head2.Code,
                    }).OrderByDescending(head2 => head2.Id).ToList()
                }).ToList();
            if (head1List?.Count > 0)
            {
                string code = head1List[0].Code;
                string[] codeArray = head1List.First().Head2?.Count > 0 ?
                    head1List.First().Head2.First().Code.Split('-') : code.Split('-');
                codeArray[1] = (Convert.ToInt32(codeArray[1]) + 1).ToString().PadLeft(2, '0');
                code = String.Join("-", codeArray);
                Head2 head2 = new Head2()
                {
                    Code = code,
                    Name = request.Name,
                    Type = request.Type,
                    Head1Id = request.Head1Id
                };
                _context.Head2.Add(head2);
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(head2);
            }
            else
            {
                throw new NotFoundException(nameof(Head1), request.Head1Id);
            }
        }
    }
}
