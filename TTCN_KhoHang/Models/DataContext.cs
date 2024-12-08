using TTCN_KhoHang.Models;
using Microsoft.EntityFrameworkCore;
namespace TTCN_KhoHang.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }
		public DbSet<User> users { get; set; }
		public DbSet<Menu> Menu { get; set; }
		public DbSet<AdminMenu> AdminMenu { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategorys { get; set; }
		public DbSet<Warehouse> Warehouses { get; set; }
		public DbSet<ImportProduct> ImportProducts { get; set; }
		public DbSet<ImportDetail> ImportDetails { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<ExportProduct> ExportProducts { get; set; }
		public DbSet<ExportDetail> ExportDetails { get; set; }
		public DbSet<Customer> Customers { get; set; }
	}
}
