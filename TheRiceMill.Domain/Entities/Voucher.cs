using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Voucher:  Base
    {
        public DateTime DateTime { get; set; }
        public VoucherType Type { get; set; }
        public VoucherDetailType DetailType { get; set; }

    }

    public enum VoucherType
    {
        Sales = 0,
        Purchase = 1
    }

    public enum VoucherDetailType
    {
        CashReceivable = 0,
        CashPayable = 1,
        BankReceivable = 2,
        BankPayable = 3,
    }
}
