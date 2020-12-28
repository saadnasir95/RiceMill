using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Heads.Models
{
    public class CreateHead1RequestModel : IRequest<ResponseViewModel>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public HeadType HeadType { get; set; }
        public CompanyType CompanyId { get; set; }
    }
}
