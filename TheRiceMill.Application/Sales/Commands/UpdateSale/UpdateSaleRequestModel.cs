using System;
using MediatR;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sales.Commands.CreateSale;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Sales.Commands.UpdateSale
{

    public class UpdateSaleRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public int[] GatepassIds { get; set; }
        public int RateBasedOn { get; set; }
        public double TotalMaund { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public DateTime Date { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
    }
}