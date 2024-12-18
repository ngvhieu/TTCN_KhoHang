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
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Order { get; set; }
		public DbSet<OrderDetail> OrderDetail { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExportProduct>()
    .HasMany(m => m.exportdetails) // Thiết lập mối quan hệ một-nhiều với ExportDetails
    .WithOne() // Thiết lập mối quan hệ ngược lại từ ExportDetails đến ExportProduct
    .HasForeignKey(d => d.export_id); // Khóa ngoại trong ExportDetails trỏ đến ExportProduct

            base.OnModelCreating(modelBuilder);
        }
    }
	
}
