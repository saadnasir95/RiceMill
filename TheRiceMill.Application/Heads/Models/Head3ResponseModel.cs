using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Heads.Models
{
    public class Head3ResponseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head2Id { get; set; }
        public List<Head4ResponseModel> Head4 { get; set; }
    }
}
