using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.Serialization;
namespace TTCN_KhoHang.Models
{
	[Table("Order")]
	[PrimaryKey(nameof(order_id))]				 
	public class Order
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int order_id { get; set; }
		public int user_id { get; set; }
		public DateTime order_date { get; set; }
		public decimal? total_amount { get; set; }
		public string order_status { get; set; }
	}
}
