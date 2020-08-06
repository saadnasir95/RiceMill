using System;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Purchases.Commands.UpdatePurchase
{

    public class UpdatePurchaseRequestModel : IRequest<ResponseViewModel>
    {
        public int Id { get; set; }
        public int[] GatepassIds { get; set; }

        //public double BagQuantity { get; set; }
        //public double BagWeight { get; set; }
        //public double ExpectedBagWeight { get; set; }
        //public double TotalExpectedBagWeight { get; set; }
        //public double KandaWeight { get; set; }
        //public double ActualBagWeight { get; set; }
        //public double TotalActualBagWeight { get; set; }
        public int RateBasedOn { get; set; }
        public double TotalMaund { get; set; }
        public double BoriQuantity { get; set; }

        //public double RatePerKg { get; set; }
        public double RatePerMaund { get; set; }
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public DateTime Date { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
    }
}