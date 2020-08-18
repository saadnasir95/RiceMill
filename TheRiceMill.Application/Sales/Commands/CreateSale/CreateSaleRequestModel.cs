using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Sales.Commands.CreateSale
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
        public int[] GatepassIds { get; set; }
        public double TotalMaund { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public int RateBasedOn { get; set; }
        public double Rate { get; set; }
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public DateTime Date { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
        public CompanyType CompanyId { get; set; }
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