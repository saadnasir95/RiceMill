using System.Collections.Generic;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sales.Commands.CreateSale;

namespace TheRiceMill.Application.Purchases.Shared
{
    public class PurchaseResponseViewModel
    {
        public int Id { get; set; }
        public string CheckIn { get; set; }
        public int VehicleId { get; set; }
        public int ProductId { get; set; }
        public List<GatePassResponseModel> Gatepasses { get; set; }
        public int RateBasedOn { get; set; }

        public VehicleRequestModel Vehicle { get; set; }
        public ProductRequestModel Product { get; set; }
        public PartyRequestModel Party { get; set; }
        public double BagQuantity { get; set; }
        public double BoriQuantity { get; set; }

        public double BagWeight { get; set; }
        public double ExpectedBagWeight { get; set; }
        public double TotalExpectedBagWeight { get; set; }
        public double KandaWeight { get; set; }
        public double ExpectedEmptyBagWeight { get; set; }
        public double TotalExpectedEmptyBagWeight { get; set; }
        public double ActualBagWeight { get; set; }
        public double TotalActualBagWeight { get; set; }
        public double ActualBags { get; set; }
        public double TotalMaund { get; set; }
        public double RatePerKg { get; set; }
        public double Rate { get; set; }

        public double BasePrice { get; set; }
        public double TotalPrice { get; set; }
        public double Commission { get; set; }
        public double PercentCommission { get; set; }
        public ChargeRequestViewModel[] AdditionalCharges { get; set; }
        public int Direction { get; set; }
        public double Vibration { get; set; }
        public string CreatedDate { get; set; }
        public string Date { get; set; }

    }

}