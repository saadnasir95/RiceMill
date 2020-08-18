﻿namespace TheRiceMill.Application.Enums
{
    public enum GatePassType
    {
        None,
        /// <summary>
        /// Sale
        /// </summary>
        OutwardGatePass,
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
    public enum CompanyType
    {
        ABRiceMill = 1,
        GDTrading = 2
    }
}