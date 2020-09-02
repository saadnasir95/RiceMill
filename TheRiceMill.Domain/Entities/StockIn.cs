using System;
using System.Collections.Generic;
using System.Text;

namespace TheRiceMill.Domain.Entities
{
    public class StockIn
    {
        public int Id { get; set; }
        public int LotId { get; set; }
        public int LotYear { get; set; }
        public Lot Lot { get; set; }
        public double BoriQuantity { get; set; }
        public double BagQuantity { get; set; }
        public double TotalKG { get; set; }
        public DateTime GatepassTime { get; set; }
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
