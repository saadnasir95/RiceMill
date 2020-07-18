using Microsoft.AspNetCore.Identity;

namespace TheRiceMill.Domain.Entities
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public Role Role { get; set; }
    }
}