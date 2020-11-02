using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheRiceMill.Application.Vehicles.Models;
using TheRiceMill.Application.Voucher.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;

namespace TheRiceMill.Application.Voucher.Commands
{
    class CreateVoucherRequestHandler: IRequestHandler<CreateVoucherRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public CreateVoucherRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(CreateVoucherRequestModel request, CancellationToken cancellationToken)
        {
            TheRiceMill.Domain.Entities.Voucher voucher = new TheRiceMill.Domain.Entities.Voucher();
            voucher.DateTime = request.DateTime;
            voucher.CompanyId = (int)request.CompanyId;
            voucher.Type = (VoucherType)request.Type;
            voucher.DetailType = (VoucherDetailType)request.DetailType;
            _context.Voucher.Add(voucher);
            _context.SaveChanges();

            request.VoucherDetails.ForEach(voucherDetail =>
            {
                TheRiceMill.Domain.Entities.VoucherDetail voucherDetailEntitiy = new TheRiceMill.Domain.Entities.VoucherDetail();
                voucherDetailEntitiy.VoucherId = voucher.Id;
                voucherDetailEntitiy.Credit = voucherDetail.Credit;
                voucherDetailEntitiy.Debit = voucherDetail.Debit;
                voucherDetailEntitiy.Remarks = voucherDetail.Remarks;
                voucherDetailEntitiy.PartyId = voucherDetail.PartyId;
                voucherDetailEntitiy.SaleId = voucherDetail.SalesId != 0 ? voucherDetail.SalesId: 0;
                voucherDetailEntitiy.PurchaseId = voucherDetail.PurchaseId != 0 ? voucherDetail.PurchaseId : 0;
                _context.VoucherDetail.Add(voucherDetailEntitiy);
            });
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseViewModel().CreateOk(new CreateVoucherResponseModel()
            {
                Id = voucher.Id,
                DateTime = voucher.DateTime,
                Type = voucher.Type,
                DetailType = request.DetailType,
        });
        }
    }
}
