using System;
using TheRiceMill.Domain.Interfaces;

namespace TheRiceMill.Domain.Entities
{
    /// <summary>
    /// Base Entity
    /// </summary>
    public class Base : IBase
    {
        /// <summary>
        /// Id of the Entity
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Date it was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The Date it was Updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }
        /// <summary>
        /// The UserId of the User that created this entity
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// The UserId of the User that updated this entity
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}