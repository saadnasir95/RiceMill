using System;
using TheRiceMill.Domain.Interfaces;

namespace TheRiceMill.Domain.Entities
{
    public class Ledger : Base
    {
        public int LedgerType { get; set; }
        public double Amount { get; set; }
        public int PartyId { get; set; }
        public Party Party { get; set; }
        public string TransactionId { get; set; }
        public int TransactionType { get; set; }
    }
}