using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Application.Purchases.Models;
using TheRiceMill.Application.Purchases.Shared;
using TheRiceMill.Application.Sale.Commands.CreateSale;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.Purchases.Queries
{
    public class GetPurchaseRequestHandler : IRequestHandler<GetPurchaseRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetPurchaseRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public Task<ResponseViewModel> Handle(GetPurchaseRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var purchase = _context.Purchases.GetMany(
                    p => p.Product.Name.Contains(request.Search),
                    request.OrderBy, request.Page, request.PageSize, request.IsDescending,
                    p => p.Include(pr => pr.Product).Include(pr => pr.Vehicle)
                        .Include(pr => pr.Party).Include(pr => pr.Charges))
                .Select(p => new
                {
                    Vehicle = new VehicleRequestModel()
                    {
                        PlateNo = p.Vehicle.PlateNo
                    },
                    Product = new ProductRequestModel()
                    {
                        Name = p.Product.Name
                    },
                    Party = new PartyRequestModel()
                    {
                        Address = p.Party.Address,
                        Name = p.Party.Name,
                        PhoneNumber = p.Party.PhoneNumber
                    },
                    PartyId = p.PartyId,
                    VehicleId = p.Vehicle.Id,
                    ProductId = p.Product.Id,
                    BagQuantity = p.BagQuantity,
                    ActualBagWeight = p.ActualBagWeight,
                    RatePerKg = p.RatePerKg,
                    RatePerMaund = p.RatePerMaund,
                    TotalPrice = p.TotalPrice,
                    ExpectedBagWeight = p.ExpectedBagWeight,
                    TotalActualBagWeight = p.TotalActualBagWeight,
                    ExpectedEmptyBagWeight = p.ExpectedEmptyBagWeight,
                    KandaWeight = p.KandaWeight,
                    Vibration = p.Vibration,
                    TotalMaund = p.TotalMaund,
                    TotalExpectedEmptyBagWeight = p.TotalExpectedEmptyBagWeight,
                    TotalExpectedBagWeight = p.TotalExpectedBagWeight,
                    Direction = p.Direction,
                    CheckIn = new DateConverter().ConvertToDateTimeIso(p.CreatedDate),
                    CreatedDate = new DateConverter().ConvertToDateTimeIso(p.CreatedDate),
                    Commission = p.Commission,
                    Id = p.Id,
                    ActualBags = p.ActualBags,
                    AdditionalCharges = p.Charges.Select(pr => new ChargeRequestViewModel()
                    {
                        Id = pr.Id,
                        Rate = pr.Rate,
                        Task = pr.Task,
                        Total = pr.Total,
                        BagQuantity = pr.BagQuantity,
                        AddPrice = pr.AddPrice,
                    }).ToArray(),
                    BagWeight = p.BagWeight,
                    BasePrice = p.BasePrice,
                    PercentCommission = p.PercentCommission
                }).ToList();
            var count = _context.Purchases.Count(p =>
                p.Product.Name.Contains(request.Search) || p.Party.Name.Contains(request.Search));
            return Task.FromResult(new ResponseViewModel().CreateOk(purchase, count));
        }
    }
}