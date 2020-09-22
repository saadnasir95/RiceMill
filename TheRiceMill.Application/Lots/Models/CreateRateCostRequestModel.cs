using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Lots.Models
{
    public class CreateRateCostRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public double LabourUnloadingAndLoading { get; set; }
        public double Freight { get; set; }
        public double PurchaseBrokery { get; set; }
        public double Total { get; set; }
        public double RatePer40WithoutProcessing { get; set; }
        public double ProcessingExpense { get; set; }
        public double BardanaAndMisc { get; set; }
        public double GrandTotal { get; set; }
        public double RatePer40LessByProduct { get; set; }
        public double SaleBrockery { get; set; }
    }
}
