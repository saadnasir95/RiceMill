using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using MediatR;
using TheRiceMill.Application.Extensions;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Lots.Models
{
    public class UpdateProcessedMaterialRequestModel : IRequest<ResponseViewModel>
    {
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public List<ProcessedMaterialRequest> ProcessedMaterials { get; set; }
    }
    public class UpdateProcessedMaterialRequestModelValidator : AbstractValidator<UpdateProcessedMaterialRequestModel>
    {
        public UpdateProcessedMaterialRequestModelValidator()
        {
            //RuleFor(p => p.ProcessedMaterials).NotNull();
            RuleFor(p => p.LotId).Required();
            RuleFor(p => p.LotYear).Required();
        }

    }
}
