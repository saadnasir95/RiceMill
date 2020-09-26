using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Lots.Models
{
    class ProcessedMaterialResponseModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public CompanyType CompanyId { get; set; }
        public Lot Lot { get; set; }
        public List<ProcessedMaterialRequest> ProcessedMaterials { get; set; }
    }
}
