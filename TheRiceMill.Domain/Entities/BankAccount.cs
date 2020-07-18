using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class BankAccount : Base
    {
        public int BankId { get; set; }
        public Bank Bank { get; set; }
        public string AccountNumber { get; set; }
        public double CurrentBalance { get; set; }
        public IEnumerable<BankTransaction> BankTransactions { get; set; }
    }
}