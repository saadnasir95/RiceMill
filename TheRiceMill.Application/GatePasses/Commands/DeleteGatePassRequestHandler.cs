using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.GatePasses.Commands
{

    public class DeleteGatePassRequestHandler : IRequestHandler<DeleteGatePassRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public DeleteGatePassRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(DeleteGatePassRequestModel request, CancellationToken cancellationToken)
        {
            var gatePass = _context.GatePasses.GetBy(p => p.Id == request.Id);
            if (gatePass == null)
            {
                throw new NotFoundException(nameof(GatePass), request.Id);
            }
            _context.GatePasses.Remove(gatePass);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }

    }

}