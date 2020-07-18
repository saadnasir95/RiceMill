using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Bank : Base
    {
        public string Name { get; set; }
        public IEnumerable<BankAccount> BankAccounts { get; set; }
    }
}