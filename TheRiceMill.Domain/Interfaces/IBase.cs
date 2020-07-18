using System;

namespace TheRiceMill.Domain.Interfaces
{
    public interface IBase
    {
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
    }
}