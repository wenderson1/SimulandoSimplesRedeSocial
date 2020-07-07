using Microsoft.EntityFrameworkCore;
using SimulandoRedeSocial.Model;

namespace SimulandoRedeSocial.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Friends> Friends { get; set; }
    }
}
