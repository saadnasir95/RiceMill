namespace TheRiceMill.Application.Enums
{
    public enum ProductType
    {
        Sale = 1,
        Purchase
    }

    public enum GatePassType
    {
        /// <summary>
        /// Sale
        /// </summary>
        OutwardGatePass = 1,
        /// <summary>
        /// Purchase
        /// </summary>
        InwardGatePass
    }

    public enum Direction
    {
        Milling = 1,
        Stockpile,
        Outside
    }
}