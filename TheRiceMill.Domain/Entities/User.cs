using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TheRiceMill.Domain.Interfaces;

namespace TheRiceMill.Domain.Entities
{
    public class User : IdentityUser, IBase
    {

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
        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<UserRole> Roles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<UserClaim> Claims { get; set; }
    }
}