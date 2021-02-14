using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head2
    {
        public new int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head1Id { get; set; }
        public Head1 Head1 { get; set; }
        public ICollection<Head3> Head3 { get; set; }
    }
}
