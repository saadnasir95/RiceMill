using System;
using System.Collections.Generic;
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
            gatePasses.ForEach(_gatepass =>
            {
                if (_gatepass.Purhcase != null) {
                    gatePassResponseModels.Add(new LotPurchaseRequestModel
                    {
                        Maund = _gatepass.Maund,
                        Party = new PartyRequestModel()
                        {
                            Name = _gatepass.Party?.Name,
                            Address = _gatepass.Party?.Address,
                            PhoneNumber = _gatepass.Party?.PhoneNumber
                        },
                        Vehicle = new VehicleRequestModel()
                        {
                            PlateNo = _gatepass.Vehicle?.PlateNo
                        },
                        BrokerName = _gatepass.Broker,
                        Freight = _gatepass.Purhcase.Freight,
                        NetWeight = _gatepass.NetWeight,
                        Date = _gatepass.Purhcase.Date.ToString(),
                        GatepassId = _gatepass.Id,
                        Total = _gatepass.Purhcase.TotalPrice,
                        BoriBag = _gatepass.BagQuantity,
                        Brokery = _gatepass.Purhcase.Commission 

                     });
                }               
            });
            return gatePassResponseModels;
        }

        public List<LotSaleRequestModel> MapGatepassToLotSale(List<GatePass> gatePasses)
        {
            List<LotSaleRequestModel> gatePassResponseModels = new List<LotSaleRequestModel>();
            gatePasses.ForEach(_gatepass =>
            {
                if (_gatepass.Sale != null)
                {

                    gatePassResponseModels.Add(new LotSaleRequestModel
                    {
                        Maund = _gatepass.Maund,
                        Party = new PartyRequestModel()
                        {
                            Name = _gatepass.Party?.Name,
                            Address = _gatepass.Party?.Address,
                            PhoneNumber = _gatepass.Party?.PhoneNumber
                        },
                        InvoiceNo = _gatepass.Sale.Id,
                        RatePer40 = _gatepass.BagQuantity,
                        BrokerName = _gatepass.Broker,
                        NetWeight = _gatepass.NetWeight,
                        Date = _gatepass.Sale.Date.ToString(),
                        GatepassId = _gatepass.Id,
                        Total = _gatepass.Sale.TotalPrice,
                        BoriBag = _gatepass.BagQuantity,
                        Brokery = _gatepass.Sale.Commission

                    });
                }
            });
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
