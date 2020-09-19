using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Lots.Commands
{
    class CreateRateCostRequestHandler: IRequestHandler<CreateRateCostRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateRateCostRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateRateCostRequestModel request,
            CancellationToken cancellationToken)
        {
            var _rateCost = new Domain.Entities.RateCost();
            _rateCost.BardanaAndMisc = request.BardanaAndMisc;
            _rateCost.LotId = request.LotId;
            _rateCost.LotYear = request.LotYear;
            _rateCost.ProcessingExpense = request.ProcessingExpense;
            _rateCost.PurchaseBrokery = request.PurchaseBrokery;
            _rateCost.RatePer40LessByProduct = request.RatePer40LessByProduct;
            _rateCost.RatePer40WithoutProcessing = request.RatePer40WithoutProcessing;
            _rateCost.SaleBrockery = request.SaleBrockery;
            _rateCost.Freight= request.Freight;
            _rateCost.LabourUnloadingAndLoading = request.LabourUnloadingAndLoading;
            _rateCost.Total = request.Total;
            _rateCost.GrandTotal = request.GrandTotal;
            _context.Add(_rateCost);
            await _context.SaveChangesAsync(cancellationToken);
            request.Id = _rateCost.Id;
            return new ResponseViewModel().CreateOk(_rateCost);
        }
    }
}
