using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Constants;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Exceptions;
using TheRiceMill.Application.Sales.Shared;
using TheRiceMill.Common.Extensions;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Sales.Commands.UpdateSale
{

    public class UpdateSaleRequestHandler : IRequestHandler<UpdateSaleRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public UpdateSaleRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(UpdateSaleRequestModel request, CancellationToken cancellationToken)
        {
            var sale = _context.Sales.GetBy(p => p.Id == request.Id, p => p.Include(pr => pr.Charges));
            if (sale == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Sale), request.Id);
            }
            var partyledger = _context.Ledgers.GetBy(p => p.Id == request.Id && p.LedgerType == (int)LedgerType.Sale && p.TransactionType == TransactionType.Party.ToInt());
            var companyLedger = _context.Ledgers.GetBy(p => p.Id == request.Id && p.LedgerType == (int)LedgerType.Sale && p.TransactionType == TransactionType.Company.ToInt());
            if (partyledger == null || companyLedger == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Ledger), request.Id);
            }
            request.Copy(sale);
            if (request.AdditionalCharges != null)
            {
                if (sale.Charges != null && sale.Charges.Any())
                {
                    _context.Charges.RemoveRange(sale.Charges);
                    await _context.SaveChangesAsync(cancellationToken);
                    sale.Charges.Clear();
                }
                else
                    sale.Charges = new List<Charge>();
                foreach (var charge in request.AdditionalCharges)
                {
                    sale.Charges.Add(new Charge()
                    {
                        BagQuantity = charge.BagQuantity,
                        Rate = charge.Rate,
                        Task = charge.Task,
                        Total = charge.Total,
                        AddPrice = charge.AddPrice,
                    });
                }
            }
            _context.Sales.Update(sale);
            {

                var gatepasses = _context.GatePasses.Where(q => q.SaleId == sale.Id).ToList();
                gatepasses.ForEach(gatepass =>
                {
                    var _gatepass = _context.GatePasses.Find(gatepass.Id);
                    _gatepass.SaleId = null;
                    _context.GatePasses.Update(_gatepass);
                });

                foreach (var id in request.GatepassIds)
                {
                    var gatepass = _context.GatePasses.GetBy(q => q.Id == id);
                    gatepass.SaleId = sale.Id;
                    _context.GatePasses.Update(gatepass);
                }

                partyledger.Amount = request.TotalPrice - request.Commission;
                partyledger.Date = sale.Date;
                partyledger.CompanyId = request.CompanyId.ToInt();
                companyLedger.Amount = -request.TotalPrice;
                companyLedger.Date = sale.Date;
                companyLedger.CompanyId = request.CompanyId.ToInt();

                _context.Ledgers.Update(companyLedger);
                _context.Ledgers.Update(partyledger);

                sale.RateBasedOn = request.RateBasedOn == 1 ? RateBasedOn.Maund : RateBasedOn.Bag;
                sale.BoriQuantity = request.BoriQuantity;
                sale.BagQuantity = request.BagQuantity;
                sale.TotalMaund = request.TotalMaund;
                sale.CompanyId = request.CompanyId.ToInt();
                await _context.SaveChangesAsync(cancellationToken);
                return new ResponseViewModel().CreateOk(new SaleResponseViewModel()
                {

                    /*                    BagWeight = request.BagWeight,
                                        KandaWeight = request.KandaWeight,*/
                    TotalMaund = request.TotalMaund,
                    Id = sale.Id,
                    Date = new DateConverter().ConvertToDateTimeIso(sale.Date),
                    Commission = sale.Commission,
                    AdditionalCharges = request.AdditionalCharges,
                    TotalPrice = sale.TotalPrice,
                    Rate = sale.Rate,
                    BagQuantity = sale.BagQuantity,
                    BoriQuantity = sale.BoriQuantity,
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(sale.CreatedDate),
                    CompanyId = (CompanyType)sale.CompanyId
                });
            }
        }
    }

}