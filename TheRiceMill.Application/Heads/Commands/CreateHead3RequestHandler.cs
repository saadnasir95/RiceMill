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
    class CreateHead3RequestHandler : IRequestHandler<CreateHead3RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateHead3RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateHead3RequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Head2, bool>> query = p => p.Id == request.Head2Id;
            var head2List = _context.Head2
                .GetMany(query, "Id", 1, 1, true)
                .Select(head2 => new Head2
                {
                    Id = head2.Id,
                    Code = head2.Code,
                    Type = head2.Type,
                    Name = head2.Name,
                    Head3 = head2.Head3.Select(head3 => new Head3
                    {
                        Id = head2.Id,
                        Code = head2.Code,
                    }).OrderByDescending(head3 => head3.Id).ToList()
                }).ToList();
            if (head2List?.Count > 0)
            {
                string code = head2List[0].Code;
                string[] codeArray = head2List.First().Head3?.Count > 0 ?
                    head2List.First().Head3.First().Code.Split('-') : code.Split('-');
                codeArray[2] = (Convert.ToInt32(codeArray[2]) + 1).ToString().PadLeft(2, '0');
                code = String.Join("-", codeArray);
                Head3 head3 = new Head3()
                {
                    Code = code,
                    Name = request.Name,
                    Type = request.Type,
                    Head2Id = request.Head2Id
                };
                _context.Head3.Add(head3);
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(head3);
            }
            else
            {
                throw new NotFoundException(nameof(Head2), request.Head2Id);
            }
        }
    }
}
