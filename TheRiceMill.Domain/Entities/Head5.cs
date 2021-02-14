using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head5
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head4Id { get; set; }
        public Head4 Head4 { get; set; }
    }
}
