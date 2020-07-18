using Microsoft.AspNetCore.Identity;

namespace TheRiceMill.Domain.Entities
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public User User { get; set; }
    }
}