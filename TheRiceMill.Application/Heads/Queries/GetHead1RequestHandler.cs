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

            List<Head1ResponseModel> head1 = _context.Head1
                .GetMany(query,
                request.OrderBy, request.Page,
                request.PageSize, request.IsDescending)
                .Select(p => new Head1ResponseModel()
                {
                    Code = p.Code,
                    HeadType = (HeadType)p.Type,
                    Name = p.Name,
                    CompanyId = (CompanyType)p.CompanyId
                }).ToList();
            var count = await _context.Head1.CountAsync(query, cancellationToken);
            return new ResponseViewModel().CreateOk(head1, count);
        }
    }
}
