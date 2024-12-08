using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace TTCN_KhoHang.Models
{
	[Table("ExportDetail")]
	public class ExportDetail
	{
		[Key]
		public int export_detail_id { get; set; }
		public int export_id { get; set; }
		public int product_id { get; set; }
		public int quantity { get; set; }
		public decimal unit_price { get; set; }
		public decimal total_price { get; set; }
	}
}
