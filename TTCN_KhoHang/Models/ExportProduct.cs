using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TTCN_KhoHang.Models
{
	[Table("ExportProduct")]
	public class ExportProduct
	{
		[Key]
		public int export_id { get; set; }
		public DateTime export_date { get; set; }
		public decimal total_amount { get; set; }
		public int user_id { get; set; }
		public int warehouse_id { get; set; }
		public int customer_id { get; set; }
		public string status { get; set; }
	}
}
