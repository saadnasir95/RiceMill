using System;
using TheRiceMill.Domain.Interfaces;

namespace TheRiceMill.Domain.Entities
{
    public class Ledger : IBase
    {
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int TransactionId { get; set; }
        public int LedgerType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}