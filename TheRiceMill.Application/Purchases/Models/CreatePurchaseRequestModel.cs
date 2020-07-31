using System;
using MediatR;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Common.Response;

namespace TheRiceMill.Application.Purchases.Models
{

    public class CreatePurchaseRequestModel : IRequest<ResponseViewModel>
    {
            public int[] GatepassIds { get; set; }
            public double TotalMaund { get; set; }
            public double RatePerMaund { get; set; }
            public double TotalPrice { get; set; }
            public double Commission { get; set; }
            public DateTime Date { get; set; }
            public ChargeRequestViewModel[] AdditionalCharges { get; set; }
        }
}