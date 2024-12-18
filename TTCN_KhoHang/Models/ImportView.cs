using TTCN_KhoHang.Models;

namespace TTCN_KhoHang.ViewModels
{
	public class ImportView
	{
		public int import_id { get; set; }
		public DateTime import_date { get; set; }
		public decimal total_amount {  get; set; }
		public string supplier_name { get; set; }
		public string warehouse_name { get; set; }
		public string user_name { get; set; }
		public string status { get; set; }
		public int product_id { get;set; }
		public string name { get; set; }
		public int quantity { get; set; }
		public decimal unit_price { get; set; }
		public decimal total_price { get; set; }
		public int supplier_id { get; set; }
		public int warehouse_id { get; set; }
		public int user_id { get; set; }
		public int category_id { get; set; }
		public string categoryname { get; set; }

		public ImportProduct ImportProduct { get; set; }
		public List<ImportDetail> ImportDetails { get; set; }
		public List<Supplier> Suppliers { get; set; }
		public List<Warehouse> Warehouses { get; set; }
		public List<Product> Products { get; set; }
		public List<ProductCategory>? ProductCategories { get; set; }
	}
}
