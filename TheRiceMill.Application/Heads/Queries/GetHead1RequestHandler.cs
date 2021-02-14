using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Heads.Models;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Heads.Queries
{
    class GetHead1RequestHandler : IRequestHandler<GetHead1RequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetHead1RequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseViewModel> Handle(GetHead1RequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            Expression<Func<Head1, bool>> query = p => p.CompanyId == request.CompanyId.ToInt();

            List<Head1ResponseModel> response = _context.Head1
                .GetMany(query,
                request.OrderBy, request.Page,
                request.PageSize, request.IsDescending)
                .Select(head1 => new Head1ResponseModel()
                {
                    Id = head1.Id,
                    Code = head1.Code,
                    Type = head1.Type,
                    Name = head1.Name,
                    CompanyId = (CompanyType)head1.CompanyId,
                    Head2 = head1.Head2.Select(head2 => new Head2ResponseModel
                    {
                        Id = head2.Id,
                        Code = head2.Code,
                        Name = head2.Name,
                        Head1Id = head2.Head1Id,
                        Type = head2.Type,
                        Head3 = head2.Head3.Select(head3 => new Head3ResponseModel
                        {
                            Id = head3.Id,
                            Code = head3.Code,
                            Name = head3.Name,
                            Head2Id = head3.Head2Id,
                            Type = head3.Type,
                            Head4 = head3.Head4.Select(head4 => new Head4ResponseModel
                            {
                                Id = head4.Id,
                                Code = head4.Code,
                                Name = head4.Name,
                                Head3Id = head4.Head3Id,
                                Type = head4.Type,
                                Head5 = head4.Head5.Select(head5 => new Head5ResponseModel
                                {
                                    Id = head5.Id,
                                    Code = head5.Code,
                                    Name = head5.Name,
                                    Head4Id = head5.Head4Id,
                                    Type = head5.Type,
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();
            var count = await _context.Head1.CountAsync(query, cancellationToken);
            return new ResponseViewModel().CreateOk(response, count);
        }
    }
}
