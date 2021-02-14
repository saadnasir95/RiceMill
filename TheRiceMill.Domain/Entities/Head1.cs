using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head1 : Base
    {
        public new int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public ICollection<Head2> Head2 { get; set; }

    }


    public enum HeadType
    {
        BalanceSheet = 0,
        ProfitAndLoss = 1,
    }
}
