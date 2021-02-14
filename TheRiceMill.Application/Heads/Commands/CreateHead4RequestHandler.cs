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
    class CreateHead4RequestHandler : IRequestHandler<CreateHead4RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateHead4RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateHead4RequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Head3, bool>> query = p => p.Id == request.Head3Id;
            var head3List = _context.Head3
                .GetMany(query, "Id", 1, 1, true)
                .Select(head3 => new Head3
                {
                    Id = head3.Id,
                    Code = head3.Code,
                    Type = head3.Type,
                    Name = head3.Name,
                    Head4 = head3.Head4.Select(head4 => new Head4
                    {
                        Id = head4.Id,
                        Code = head4.Code,
                    }).OrderByDescending(head4 => head4.Id).ToList()
                }).ToList();
            if (head3List?.Count > 0)
            {
                string code = head3List[0].Code;
                string[] codeArray = head3List.First().Head4?.Count > 0 ?
                    head3List.First().Head4.First().Code.Split('-') : code.Split('-');
                codeArray[3] = (Convert.ToInt32(codeArray[3]) + 1).ToString().PadLeft(2, '0');
                code = String.Join("-", codeArray);
                Head4 head4 = new Head4()
                {
                    Code = code,
                    Name = request.Name,
                    Type = request.Type,
                    Head3Id = request.Head3Id
                };
                _context.Head4.Add(head4);
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(head4);
            }
            else
            {
                throw new NotFoundException(nameof(Head3), request.Head3Id);
            }
        }
    }
}
