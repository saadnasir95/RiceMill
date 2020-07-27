using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Company : Base
    {
        /// <summary>
        /// Name of the Company e.g "Apple"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NormalizedName of the Company e.g "APPLE"
        /// </summary>
        public string NormalizedName { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<GatePass> GatePasses { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
        public IEnumerable<BankTransaction> BankTransactions { get; set; }
        public IEnumerable<Ledger> Ledgers { get; set; }
    }
}