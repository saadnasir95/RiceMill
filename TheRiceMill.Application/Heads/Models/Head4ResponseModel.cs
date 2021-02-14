using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Heads.Models
{
    public class Head4ResponseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType Type { get; set; }
        public int Head3Id { get; set; }
        public List<Head5ResponseModel> Head5 { get; set; }
    }
}
