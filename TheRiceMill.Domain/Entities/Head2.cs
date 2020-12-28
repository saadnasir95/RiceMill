using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head2: Base
    {
        [Key]
        public new int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public ICollection<Head1> Head1 { get; set; }

    }
}
