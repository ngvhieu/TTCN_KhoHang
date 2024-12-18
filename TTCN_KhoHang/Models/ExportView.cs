using TTCN_KhoHang.Models;
namespace TTCN_KhoHang.ViewModels
{
	public class ExportView
	{
		public int export_id { get; set; }
		public DateTime export_date { get; set; }
		public decimal? total_amount { get; set; }
		public int customer_id { get; set; }
		public int warehouse_id { get; set; }
		public int user_id { get; set; }
		public string status { get; set; }

		// Navigation properties
		public string customer_name { get; set; }
		public string warehouse_name { get; set; }
		public string user_name { get; set; }

		// Product details
		public int product_id { get; set; }
		public string name { get; set; }
		public int quantity { get; set; }
		public decimal? unit_price { get; set; }
		public decimal? total_price { get; set; }

		// Lists for dropdowns
		public List<Customer> Customers { get; set; }
		public List<Warehouse> Warehouses { get; set; }
		public List<Product> Products { get; set; }
		public List<ExportDetail> ExportDetails { get; set; }
	}
}
