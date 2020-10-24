using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Lots.Models
{
    public class GetLotResponseModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public List<StockInRequestModel> StockIns { get; set; }
        public List<StockOutRequestModel> StockOuts { get; set; }
        public List<ProcessedMaterialRequest> ProcessedMaterials { get; set; }
        public List<RateCostRequestModel> RateCosts { get; set; }
        public List<LotPurchaseRequestModel> Purchases { get; set; }
        public List<LotSaleRequestModel> Sales { get; set; }

    }
    public class StockInRequestModel
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double TotalKG { get; set; }
        public DateTime GatepassTime { get; set; }
    }
    public class StockOutRequestModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductRequestModel Product { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double PerKG { get; set; }
        public double TotalKG { get; set; }
    }
    public class RateCostRequestModel
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
        public double SaleBrokery { get; set; }
    }


    public class LotPurchaseRequestModel
    {
        public string Date { get; set; }
        public string BrokerName { get; set; }
        public string Party { get; set; }
        public string GatepassIds { get; set; }
        public string Vehicle { get; set;  }
        public int? PurchaseId { get; set; }
        public double Bori { get; set; }
        public double Bag { get; set; }

        public double NetWeight { get; set; }
        public double Maund { get; set; }
        public double RatePer40 { get; set; }
        public double Total { get; set; }
        public double Freight { get; set; }
        public double Brokery { get; set; }

    }

    public class LotSaleRequestModel
    {
        public string Date { get; set; }
        public string BrokerName { get; set; }
        public string Party { get; set; }
        public string GatepassIds { get; set; }
        public double Bag { get; set; }
        public double Bori { get; set; }
        public int Type { get; set; }
        public int? SaleId { get; set; }
        public double NetWeight { get; set; }
        public double Maund { get; set; }
        public double RatePer40 { get; set; }
        public double Total { get; set; }
        public int InvoiceNo { get; set; }
        public double Brokery { get; set; }

    }
}
