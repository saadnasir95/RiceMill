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
            public DateTime CheckIn { get; set; }
            public Direction Direction { get; set; }
            public int CompanyId { get; set; }
            public CompanyRequestModel Company { get; set; }
            public int VehicleId { get; set; }
            public VehicleRequestModel Vehicle { get; set; }
            public int ProductId { get; set; }
            public ProductRequestModel Product { get; set; }
            public double BagQuantity { get; set; }
            public double BagWeight { get; set; }
            public double ExpectedBagWeight { get; set; }
            public double TotalExpectedBagWeight { get; set; }
            public double KandaWeight { get; set; }
            public double ExpectedEmptyBagWeight { get; set; }
            public double TotalExpectedEmptyBagWeight { get; set; }
            public double Vibration { get; set; }
            public double ActualBagWeight { get; set; }
            public double TotalActualBagWeight { get; set; }
            public double TotalMaund { get; set; }
            public double RatePerKg { get; set; }
            public double RatePerMaund { get; set; }
            public double BasePrice { get; set; }
            public double TotalPrice { get; set; }
            public double ActualBags { get; set; }
            public string BiltyNumber { get; set; }
            public double Commission { get; set; }
            public double PercentCommission { get; set; }
            public ChargeRequestViewModel[] AdditionalCharges { get; set; }
        }
}