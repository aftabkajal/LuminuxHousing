using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AppCore.Entities;


namespace AppInfrastructure.Data
{
    public class LuminuxContext : DbContext
    {
        public LuminuxContext(DbContextOptions<LuminuxContext> options) : base(options)
        {

        }

        public DbSet<Plots> Plots { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
    }
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation

    public class LuminuxContextDesignFactory : IDesignTimeDbContextFactory<LuminuxContext>
    {
        public LuminuxContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LuminuxContext>().UseSqlServer("Server=.;Initial Catalog=LuminuxDb;Integrated Security=true");

            return new LuminuxContext(optionsBuilder.Options);
        }
    }
}
