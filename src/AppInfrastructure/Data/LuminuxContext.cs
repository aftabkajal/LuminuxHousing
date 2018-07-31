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

}
