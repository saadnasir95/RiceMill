using System;

namespace TheRiceMill.Domain.Entities
{
    public class BankTransaction : Base
    {
        public double Credit { get; set; }
        public double Debit { get; set; }
        public int TransactionType { get; set; }
        public int PaymentType { get; set; }
        public DateTime TransactionDate { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string ChequeNumber { get; set; }
    }
}