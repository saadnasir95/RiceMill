using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Constants;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Lots.Models
{
    public class CreateProcessedMaterialRequestModel : IRequest<ResponseViewModel>
    {
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public CompanyType CompanyId { get; set; }
        public List<ProcessedMaterialRequest> ProcessedMaterials { get; set; }
  
    }

    public class ProcessedMaterialRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double PerKG { get; set; }
        public double TotalKG { get; set; }
    }

    public class CreateProcessedMaterialModelValidator : AbstractValidator<CreateProcessedMaterialRequestModel>
    {
        public CreateProcessedMaterialModelValidator()
        {
            //RuleFor(p => p.ProcessedMaterials).NotNull();
            RuleFor(p => p.LotId).Required();
            RuleFor(p => p.LotYear).Required();
            RuleFor(p => p.CompanyId).IsInEnum();
        }

    }
}
