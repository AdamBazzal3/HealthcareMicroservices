using Microsoft.EntityFrameworkCore;
using HealthcareIdentityAPI.Models;

namespace HealthcareIdentityAPI.Data
{
    public class DataContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
    }
}
