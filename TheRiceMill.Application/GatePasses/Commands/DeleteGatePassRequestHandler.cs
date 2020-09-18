using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
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
            else if (gatePass != null && (gatePass.SaleId != null || gatePass.PurchaseId != null))
            {
                throw new CannotDeleteException(nameof(GatePass), request.Id);
            }
            else
            {
                Lot lot = _context.Lots.GetBy(c => c.Id == gatePass.LotId && c.Year == gatePass.LotYear, c => c.Include(d => d.StockIns).Include(d => d.StockOuts));
                if (lot == null)
                {
                    throw new NotFoundException(nameof(Lot), gatePass.LotId);
                }
                if ((GatePassType)gatePass.Type == GatePassType.InwardGatePass)
                {
                    var stockIn = lot.StockIns.FirstOrDefault(c => c.GatepassTime == gatePass.DateTime);
                    lot.StockIns.Remove(stockIn);
                }
                else
                {
                    var stockOut = lot.StockOuts.FirstOrDefault(c => c.ProductId == gatePass.ProductId);
                    stockOut.BagQuantity -= gatePass.BagQuantity;
                    stockOut.BoriQuantity -= gatePass.BoriQuantity;
                    stockOut.TotalKG -= gatePass.NetWeight;
                    stockOut.PerKG = stockOut.TotalKG / (stockOut.BoriQuantity + stockOut.BagQuantity);
                }
                _context.Lots.Update(lot);
            }
            _context.GatePasses.Remove(gatePass);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk();
        }

    }

}