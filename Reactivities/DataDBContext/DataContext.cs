using Microsoft.EntityFrameworkCore;
using Reactivities.Modules;

namespace Reactivities.DataDBContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Activity> Activities { get; set; }
    }

}
