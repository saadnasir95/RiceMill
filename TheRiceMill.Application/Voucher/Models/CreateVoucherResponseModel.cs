using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Voucher.Models
{
    class CreateVoucherResponseModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public VoucherType Type { get; set; }
        public VoucherDetailType DetailType { get; set; }

    }
}
