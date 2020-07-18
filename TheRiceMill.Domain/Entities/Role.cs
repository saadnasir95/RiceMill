using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TheRiceMill.Domain.Entities
{
    public class Role : IdentityRole
    {
        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public virtual ICollection<RoleClaim> Claims { get; set; }

    }
}