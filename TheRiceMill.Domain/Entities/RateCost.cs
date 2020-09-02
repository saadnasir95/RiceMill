using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class RateCost
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public Lot Lot { get; set; }
        public double LabourUnloadingAndLoading { get; set; }
        public double Freight { get; set; }
        public double PurchaseBrokery { get; set; }
        public double Total { get; set; }
        public double RatePer40WithoutProcessing { get; set; }
        public double ProcessingExpense { get; set; }
        public double BardanaAndMisc { get; set; }
        public double GrandTotal { get; set; }
        public double RatePer40LessByProduct { get; set; }
        public double SaleBrockery { get; set; }
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The Date it was Updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }
        /// <summary>
        /// The UserId of the User that created this entity
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// The UserId of the User that updated this entity
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
