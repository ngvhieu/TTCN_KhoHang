namespace TTCN_KhoHang.ViewModels
{
	public class UserOrderView
	{
		public int order_id { get; set; }
		public DateTime order_date { get; set; }
		public string order_status { get; set; }
		public int product_id { get; set; }
		public string product_name { get; set; }
		public int quantity { get; set; }
		public decimal? unit_price { get; set; }
		public decimal? total_price { get; set; }
		public decimal? total_amount { get; set; }
	}
}
