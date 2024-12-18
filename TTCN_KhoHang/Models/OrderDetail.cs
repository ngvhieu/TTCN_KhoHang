using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
namespace TTCN_KhoHang.Models
{
	[Table("OrderDetail")]
	[PrimaryKey(nameof(order_detail_id))]
	public class OrderDetail
	{
		public int order_detail_id { get; set; }
		[ForeignKey(name:nameof(Order))]
		public int order_id { get; set; }
		public int product_id { get; set; }
		public int quantity { get; set; }
		public decimal? unit_price { get; set; }
		public decimal? total_price { get; set; }
	}
}
