using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Sale.Commands.CreateSale
{

//    export class Sale {
//    id: number;
//    checkOut: string;
//    partyId: number;
//    party: Party;
//    vehicleId: number;
//    vehicle: Vehicle;
//    productId: number;
//    product: Product;
//    bagQuantity: number;
//    bagWeight: number;
//    expectedBagWeight: number;
//    totalExpectedBagWeight: number;
//    kandaWeight: number;
//    expectedEmptyBagWeight: number;
//    totalExpectedEmptyBagWeight: number;
//    actualBagWeight: number;
//    totalActualBagWeight: number;
//    totalMaund: number;
//    ratePerKg: number;
//    ratePerMaund: number;
//    basePrice: number;
//    totalPrice: number;
//    biltyNumber: string;
//    commission: number;
//    percentCommission: number;
//    additionalCharges: AdditionalCharges[];
//}
    public class CreateSaleRequestModel : IRequest<ResponseViewModel>
    {
        public DateTime CheckOut { get; set; }
        public int PartyId { get; set; }
        public PartyRequestModel Party { get; set; }
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
        public double ActualBagWeight { get; set; }
        public double TotalActualBagWeight { get; set; }
        public double TotalMaund { get; set; }
        public double RatePerKg { get; set; }
        public double RatePerMaund { get; set; }
        public double BasePrice { get; set; }
        public double TotalPrice { get; set; }
        public string BiltyNumber { get; set; }
        public double Commission { get; set; }
        public double PercentCommission { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
    }

    public class ChargeRequestViewModel
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public int BagQuantity { get; set; }
        public double Rate { get; set; }
        public double Total { get; set; }
        public bool AddPrice { get; set; }
    }
}