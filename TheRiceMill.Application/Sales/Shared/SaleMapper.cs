using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Sales.Shared;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.Purchases.Shared
{
    public class SaleMapper
    {
        public SaleMapper()
        {

        }

        public SaleResponseViewModel MapForGatePass(Sale purchase)
        {
            SaleResponseViewModel sale = new SaleResponseViewModel();
            sale.TotalMaund = purchase.TotalMaund;
            sale.BagQuantity = purchase.BagQuantity;
            sale.BoriQuantity = purchase.BoriQuantity;
            sale.Id = purchase.Id;
            sale.BasePrice = purchase.BasePrice;
            sale.Freight = purchase.Freight;
            sale.RateBasedOn = (int)purchase.RateBasedOn;
            sale.Commission = purchase.Commission;
            sale.TotalPrice = purchase.TotalPrice;
            sale.Rate = purchase.Rate;
            sale.Date = new DateConverter().ConvertToDateTimeIso(purchase.Date);
            sale.CreatedDate = new DateConverter().ConvertToDateTimeIso(purchase.CreatedDate);
            sale.CompanyId = (CompanyType)purchase.CompanyId;
            return sale;
        }


    }
}
