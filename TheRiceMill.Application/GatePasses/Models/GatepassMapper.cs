using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.Lots.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sales.Shared;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class GatepassMapper
    {
        PurchaseMapper purchaseMapper;
        SaleMapper saleMapper;

        public GatepassMapper() {
            saleMapper = new SaleMapper();
            purchaseMapper = new PurchaseMapper();
        }

        public List<LotPurchaseRequestModel> MapGatepassToLotPurchase(List<GatePass> gatePasses)
        {
            List<LotPurchaseRequestModel> gatePassResponseModels = new List<LotPurchaseRequestModel>();
            if (gatePasses != null)
            {
                gatePassResponseModels = gatePasses.Where(x => x.PurchaseId != null).GroupBy(u => u.PurchaseId)
                    .Select(grp => new LotPurchaseRequestModel
                    {
                        Maund = grp.Select(g => g.Maund).FirstOrDefault(),
                        Party = string.Join(",", grp.Select(g => g.Party.Name)),
                        Vehicle = string.Join(",", grp.Select(g => g.Vehicle.PlateNo)),
                        PurchaseId = grp.Select(g => g.PurchaseId).FirstOrDefault(),
                        BrokerName = string.Join(",", grp.Select(g => g.Broker)),
                        Freight = grp.Sum(g => g.Purhcase.Freight),
                        NetWeight = grp.Sum(g => g.NetWeight),
                        Date = grp.Select(g => g.Purhcase.Date).FirstOrDefault().ToString(),
                        GatepassIds = string.Join(",", grp.Select(g => g.Id)),
                        Total = grp.Sum(g => g.Purhcase.TotalMaund),
                        Bag = grp.Sum(g => g.BagQuantity),
                        Bori = grp.Sum(g => g.BoriQuantity),
                        Brokery = grp.Select(g => g.Purhcase.Commission).FirstOrDefault()
                    })
                    .ToList();
            }
            return gatePassResponseModels;
        }

        public List<LotSaleRequestModel> MapGatepassToLotSale(List<GatePass> gatePasses)
        {
            List<LotSaleRequestModel> gatePassResponseModels = new List<LotSaleRequestModel>();
            if(gatePasses != null)
            {
                gatePassResponseModels = gatePasses.Where(x => x.SaleId != null).GroupBy(u => u.SaleId)
                 .Select(grp => new LotSaleRequestModel
                 {
                     Maund = grp.Select(g => g.Maund).FirstOrDefault(),
                     Party = string.Join(",", grp.Select(g => g.Party.Name)),
                     BrokerName = string.Join(",", grp.Select(g => g.Broker)),
                     InvoiceNo = grp.Select(g => g.SaleId).FirstOrDefault() ?? 0,
                     NetWeight = grp.Sum(g => g.NetWeight),
                     Date = grp.Select(g => g.Sale.Date).FirstOrDefault().ToString(),
                     GatepassIds = string.Join(",", grp.Select(g => g.Id)),
                     Total = grp.Sum(g => g.Sale.TotalMaund),
                     Bag = grp.Sum(g => g.BagQuantity),
                     Type =  grp.Select(g => g.Type).FirstOrDefault(),
                     Bori = grp.Sum(g => g.BoriQuantity),
                     Brokery = grp.Select(g => g.Sale.Commission).FirstOrDefault()
                 })
                 .ToList();
            }
            return gatePassResponseModels;
        }

        public List<GatePassResponseModel> MapFull(List<GatePass> gatePasses)
        {
            List<GatePassResponseModel> gatePassResponseModels = new List<GatePassResponseModel>();
            gatePasses.ForEach(_gatepass =>
            {
                gatePassResponseModels.Add(new GatePassResponseModel
                {
                    BagQuantity = _gatepass.BagQuantity,
                    BoriQuantity = _gatepass.BoriQuantity,
                    Broker = _gatepass.Broker,
                    //LotNumber = _gatepass.LotNumber,
                    KandaWeight = _gatepass.KandaWeight,
                    EmptyWeight = _gatepass.EmptyWeight,
                    Maund = _gatepass.Maund,
                    Party = new PartyRequestModel()
                    {
                        Name = _gatepass.Party.Name,
                        Address = _gatepass.Party.Address,
                        PhoneNumber = _gatepass.Party.PhoneNumber
                    },
                    Vehicle = new VehicleRequestModel()
                    {
                        PlateNo = _gatepass.Vehicle.PlateNo
                    },
                    Product = new ProductRequestModel()
                    {
                        Name = _gatepass.Product.Name,
                        /*Price = gatepass.Product.Price,
                          Type = (ProductType)gatepass.Product.Type*/
                    },
                    VehicleId = _gatepass.Vehicle.Id,
                    ProductId = _gatepass.Product.Id,
                    Type = (GatePassType)_gatepass.Type,
                    WeightPerBag = _gatepass.WeightPerBag,
                    NetWeight = _gatepass.NetWeight,
                    DateTime = _gatepass.DateTime,
                    Id = _gatepass.Id,
                    PartyId = _gatepass.PartyId,
                    PurchaseId = _gatepass.PurchaseId,
                    SaleId = _gatepass.SaleId
                });
            });
            return gatePassResponseModels;

        }


        public GatePassResponseModel Map(GatePass _gatepass)
        {
            return new GatePassResponseModel
            {
                BagQuantity = _gatepass.BagQuantity,
                BoriQuantity = _gatepass.BoriQuantity,
                Broker = _gatepass.Broker,
                //LotNumber = _gatepass.LotNumber,
                KandaWeight = _gatepass.KandaWeight,
                EmptyWeight = _gatepass.EmptyWeight,
                Maund = _gatepass.Maund,
                Party = new PartyRequestModel()
                {
                    Name = _gatepass.Party.Name,
                    Address = _gatepass.Party.Address,
                    PhoneNumber = _gatepass.Party.PhoneNumber
                },
                Vehicle = new VehicleRequestModel()
                {
                    PlateNo = _gatepass.Vehicle.PlateNo
                },
                Product = new ProductRequestModel()
                {
                    Name = _gatepass.Product.Name,
                    /*Price = gatepass.Product.Price,
                      Type = (ProductType)gatepass.Product.Type*/
                },
                VehicleId = _gatepass.Vehicle.Id,
                ProductId = _gatepass.Product.Id,
                Type = (GatePassType)_gatepass.Type,
                WeightPerBag = _gatepass.WeightPerBag,
                NetWeight = _gatepass.NetWeight,
                DateTime = _gatepass.DateTime,
                Id = _gatepass.Id,
                PartyId = _gatepass.PartyId,
                PurchaseId = _gatepass.PurchaseId,
                SaleId = _gatepass.SaleId
            };
        }
    }
}
