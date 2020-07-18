using Microsoft.EntityFrameworkCore;
using TheRiceMill.Persistence.Infrastructures;

namespace TheRiceMill.Persistence
{
    public class TheRiceMillDbContextFactory : DesignTimeDbContextFactoryBase<TheRiceMillDbContext>
    {
        protected override TheRiceMillDbContext CreateNewInstance(DbContextOptions<TheRiceMillDbContext> options)
        {
            return new TheRiceMillDbContext(options);
        }
    }
}