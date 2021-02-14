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
    class CreateHead5RequestHandler : IRequestHandler<CreateHead5RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateHead5RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateHead5RequestModel request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Head4, bool>> query = p => p.Id == request.Head4Id;
            var head4List = _context.Head4
                .GetMany(query, "Id", 1, 1, true)
                .Select(head4 => new Head4
                {
                    Id = head4.Id,
                    Code = head4.Code,
                    Type = head4.Type,
                    Name = head4.Name,
                    Head5 = head4.Head5.Select(head5 => new Head5
                    {
                        Id = head5.Id,
                        Code = head5.Code,
                    }).OrderByDescending(head5 => head5.Id).ToList()
                }).ToList();
            if (head4List?.Count > 0)
            {
                string code = head4List[0].Code;
                string[] codeArray = head4List.First().Head5?.Count > 0 ?
                    head4List.First().Head5.First().Code.Split('-') : code.Split('-');
                codeArray[4] = (Convert.ToInt32(codeArray[4]) + 1).ToString().PadLeft(4, '0');
                code = String.Join("-", codeArray);
                Head5 head5 = new Head5()
                {
                    Code = code,
                    Name = request.Name,
                    Type = request.Type,
                    Head4Id = request.Head4Id
                };
                _context.Head5.Add(head5);
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(head5);
            }
            else
            {
                throw new NotFoundException(nameof(Head4), request.Head4Id);
            }
        }
    }
}
