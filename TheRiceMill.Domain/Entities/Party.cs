using System.Collections.Generic;

namespace TheRiceMill.Domain.Entities
{
    public class Party : Base
    {
        /// <summary>
        /// Name of the Party e.g "Apple"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// NormalizedName of the Party e.g "APPLE"
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