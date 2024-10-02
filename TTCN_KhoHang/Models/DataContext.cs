using Microsoft.EntityFrameworkCore;
namespace TTCN_KhoHang.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<User> users { get; set; }
    }
}
