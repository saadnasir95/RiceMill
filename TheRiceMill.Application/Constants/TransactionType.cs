namespace TheRiceMill.Application.Constants
{
    public enum TransactionType
    {
        /// <summary>
        /// 0 None
        /// </summary>
        None = 0,
        /// <summary>
        /// 1 Credit
        /// </summary>
        Company = 1,
        /// <summary>
        /// 2 Debit
        /// </summary>
        Party,
    }

    public enum PaymentType
    {
        None = 0,
        Cash = 1,
        Cheque = 2,
    }

    public enum LedgerType
    {
        None = 0,
        Sale = 1,
        Purchase,
        BankTransaction
    }

    public enum Bank
    {
        AskariBank = 1,
        AlliedBank = 2,
        BankAlfalah = 3,
        HabibBankLimited = 4,
        BankofPunjab = 5,
        UnitedBankLimited = 6,
        MeezanBank = 7,
        NationalBankOfPakistan = 8,
        McbBank = 9,
        StandardCharteredBank = 10
    }

}