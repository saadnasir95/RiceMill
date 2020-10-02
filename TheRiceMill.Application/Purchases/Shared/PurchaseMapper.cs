using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Purchases.Shared
{
    public class PurchaseMapper
    {
        public PurchaseMapper()
        {

        }

        public PurchaseResponseViewModel MapForGatePass(Purchase purchase)
        {
            PurchaseResponseViewModel purchaseResponseViewModel = new PurchaseResponseViewModel();
            purchaseResponseViewModel.TotalMaund = purchase.TotalMaund;
            purchaseResponseViewModel.BagQuantity = purchase.BagQuantity;
            purchaseResponseViewModel.BoriQuantity = purchase.BoriQuantity;
            purchaseResponseViewModel.Id = purchase.Id;
            purchaseResponseViewModel.BasePrice = purchase.BasePrice;
            purchaseResponseViewModel.Freight = purchase.Freight;
            purchaseResponseViewModel.RateBasedOn = (int)purchase.RateBasedOn;
            purchaseResponseViewModel.Commission = purchase.Commission;
            purchaseResponseViewModel.TotalPrice = purchase.TotalPrice;
            purchaseResponseViewModel.Rate = purchase.Rate;
            purchaseResponseViewModel.Date = new DateConverter().ConvertToDateTimeIso(purchase.Date);
            purchaseResponseViewModel.CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate);
            purchaseResponseViewModel.CompanyId = (CompanyType)purchase.CompanyId;
            return purchaseResponseViewModel;
        }


    }
}
