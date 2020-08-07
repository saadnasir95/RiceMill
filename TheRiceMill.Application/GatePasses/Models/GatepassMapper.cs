using System;
using System.Collections.Generic;
using System.Text;
using TheRiceMill.Domain.Entities;

namespace TheRiceMill.Application.GatePasses.Models
{
    public class GatepassMapper
    {
        public GatepassMapper() { }

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
            };
        }
    }
}
