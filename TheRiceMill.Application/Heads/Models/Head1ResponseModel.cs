using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Heads.Models
{
    public class Head1ResponseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType HeadType { get; set; }
        public CompanyType CompanyId { get; set; }

    }
}
