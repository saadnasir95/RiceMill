using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head4
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head3Id { get; set; }
        public Head3 Head3 { get; set; }
        public ICollection<Head5> Head5 { get; set; }
    }
}
