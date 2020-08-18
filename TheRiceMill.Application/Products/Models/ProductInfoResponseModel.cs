﻿using TheRiceMill.Application.Enums;

namespace TheRiceMill.Application.Products.Models
{
    public class ProductInfoResponseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public CompanyType CompanyId { get; set; }
        public string CreatedDate { get; set; }
    }

    public class ProductsInfoComboBoxResponseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public CompanyType CompanyId { get; set; }
    }
}