using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using TheRiceMill.Domain.Entities;
using TheRiceMill.Common.Constants;

namespace TheRiceMill.Persistence
{
    public class TheRiceMillInitializer
    {

        public static void Initialize(TheRiceMillDbContext context)
        {
            TheRiceMillInitializer initializer = new TheRiceMillInitializer();
            initializer.SeedEverything(context);
        }

        public void SeedEverything(TheRiceMillDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            SeedBank(context);
        }

        public void SeedBank(TheRiceMillDbContext context)
        {
            if (context.Banks.Any())
            {
                return;
            }

            context.Banks.Add(new Bank()
            {
                Name = "Askari Bank"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Allied Bank"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Bank Alfalah"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Habib Bank Limited"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Bank Of Punjab"
            });
            context.Banks.Add(new Bank()
            {
                Name = "United Bank Limited"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Meezan Bank"
            });
            context.Banks.Add(new Bank()
            {
                Name = "National Bank of Pakistan"
            });
            context.Banks.Add(new Bank()
            {
                Name = "MCB Bank"
            });
            context.Banks.Add(new Bank()
            {
                Name = "Standard Charted Bank"
            });
            context.SaveChanges();
        }

        public void SeedRoles(TheRiceMillDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Set<Role>().Add(new Role()
            {
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpper()
            });
            context.Set<Role>().Add(new Role()
            {
                Name = RoleNames.GateKeeper,
                NormalizedName = RoleNames.GateKeeper.ToUpper()
            });
            context.SaveChanges();
        }
        public void SeedUsers(TheRiceMillDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            PasswordHasher<User> passwordHash = new PasswordHasher<User>();
            User user = new User()
            {
                UserName = Constant.AdminUserName,
                NormalizedUserName = Constant.AdminUserName.ToUpper(),
                Email = Constant.AdminEmail,
                NormalizedEmail = Constant.AdminEmail.ToUpper(),
            };
            user.PasswordHash = passwordHash.HashPassword(user, Constant.AdminPassword);
            string adminRoleId = context.Set<Role>().FirstOrDefault(p => p.Name == RoleNames.Admin)?.Id + "";
            if (string.IsNullOrEmpty(adminRoleId))
            {
                throw new Exception(Messages.NoAdminRoleFound);
            }
            User gateKeeper = new User()
            {
                UserName = Constant.GateKeeperUserName,
                NormalizedUserName = Constant.GateKeeperUserName.ToUpper(),
                Email = Constant.GateKeeperEmail,
                NormalizedEmail = Constant.GateKeeperEmail.ToUpper(),
            };
            gateKeeper.PasswordHash = passwordHash.HashPassword(gateKeeper, Constant.GateKeeperPassword);
            string gateKeeperId = context.Set<Role>().FirstOrDefault(p => p.Name == RoleNames.GateKeeper)?.Id + "";
            if (string.IsNullOrEmpty(gateKeeperId))
            {
                throw new Exception(Messages.NoGateKeeperRoleFound);
            }

            
            context.Users.Add(user);
            context.Users.Add(gateKeeper);
            context.SaveChanges();
            context.UserRoles.Add(new UserRole()
            {
                RoleId = adminRoleId,
                UserId = user.Id,
            });
            context.UserRoles.Add(new UserRole()
            {
                RoleId = gateKeeperId,
                UserId = gateKeeper.Id,
            });
            context.SaveChanges();
        }
    }
}