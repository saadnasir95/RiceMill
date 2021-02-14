﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class Head3
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head2Id { get; set; }
        public Head2 Head2 { get; set; }
        public ICollection<Head4> Head4 { get; set; }
    }
}
