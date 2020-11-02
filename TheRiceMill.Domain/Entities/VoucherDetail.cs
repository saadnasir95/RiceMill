using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class VoucherDetail: Base
    {
        public int PartyId { get; set; }
        public Party Party { get; set; }
        public int? SaleId { get; set; }
        public Sale Sale { get; set; }
        public int? PurchaseId { get; set; }
        public Purchase Purhcase { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Remarks { get; set; }
        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; }
    }
}
