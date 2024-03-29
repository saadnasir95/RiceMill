﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheRiceMill.Application.Enums;
using TheRiceMill.Application.GatePasses.Models;
using TheRiceMill.Common.Response;
using TheRiceMill.Common.Util;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Persistence;
using TheRiceMill.Persistence.Extensions;

namespace TheRiceMill.Application.GatePasses.Queries
{

    public class GetGatePassRequestHandler : IRequestHandler<GetGatePassRequestModel, ResponseViewModel>
    {
        private readonly TheRiceMillDbContext _context;

        public GetGatePassRequestHandler(TheRiceMillDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel> Handle(GetGatePassRequestModel request, CancellationToken cancellationToken)
        {
            request.SetDefaultValue();
            var converter = new DateConverter();
            Expression<Func<GatePass,bool>> searchQuery = p => p.Company.Name.Contains(request.Search) ||
                                   (p.Id + "" == request.Search) ||
                                   p.Vehicle.PlateNo.Contains(request.Search) ||
                                   p.Product.Name.Contains(request.Search);
            List<GatePassResponseModel> gatePasses = _context.GatePasses
                .GetMany(searchQuery,
                request.OrderBy, request.Page,
                request.PageSize, request.IsDescending,
                p => p.Include(pr => pr.Company).Include(pr => pr.Vehicle).Include(pr => pr.Product))
                .Select(p => new GatePassResponseModel()
            {
                Type = (GatePassType)p.Type,
                BagQuantity = (int)p.BagQuantity,
                BagWeight = p.BagWeight,
                KandaWeight = p.KandaWeight,
                TotalMaund = p.TotalMaund,
                CheckDateTime = p.CheckIn.GetValueOrDefault(),
                Direction = (Direction)p.Direction,
                Id = p.Id,
                Company = new CompanyRequestModel()
                {
                    Address = p.Company.Address,
                    Name = p.Company.Name,
                    PhoneNumber = p.Company.PhoneNumber
                },
                Product = new ProductRequestModel()
                {
                    Name = p.Product.Name,
                    Price = p.Product.Price,
                    Type = (ProductType)p.Product.Type,
                },
                Vehicle = new VehicleRequestModel()
                {
                    PlateNo = p.Vehicle.PlateNo,
                    Name = p.Vehicle.Name,
                },
                BiltyNumber = p.BiltyNumber,
                CompanyId = p.CompanyId,
                ProductId = p.ProductId,
                VehicleId = p.VehicleId
            }).ToList();
            var count = await _context.GatePasses.CountAsync(searchQuery,cancellationToken);
            return new ResponseViewModel().CreateOk(gatePasses, count);
        }
    }

}