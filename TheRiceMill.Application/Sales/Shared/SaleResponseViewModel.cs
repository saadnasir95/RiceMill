using System;
using System.Collections.Generic;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sales.Commands.CreateSale;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Sales.Shared
{
    public class SaleResponseViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int ProductId { get; set; }
        public List<GatePassResponseModel> Gatepasses { get; set; }
        public int RateBasedOn { get; set; }

        public VehicleRequestModel Vehicle { get; set; }
        public ProductRequestModel Product { get; set; }
        public PartyRequestModel Party { get; set; }
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }
        public double TotalMaund { get; set; }
        public double RatePerKg { get; set; }
        public double Rate { get; set; }

        public double BasePrice { get; set; }
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public double Freight { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
        public string CreatedDate { get; set; }
        public string Date { get; set; }
        public CompanyType CompanyId { get; set; }
        public SaleType Type { get; set; }
    }
}