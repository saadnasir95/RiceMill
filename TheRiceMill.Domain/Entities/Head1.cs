using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head1 : Base
    {
        [Key]
        public new int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

    }


    public enum HeadType
    {
        BalanceSheet = 0,
        ProfitAndLoss = 1,
    }
}
