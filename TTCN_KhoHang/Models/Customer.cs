using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace TTCN_KhoHang.Models
{
	[Table("Customers")]
	public class Customer
	{
		[Key]
		public int customer_id { get; set; }
		public string customer_name { get; set; }
		public string customer_address { get; set; }
		public string customer_phone { get; set; }
		public string customer_email { get; set; }
	}
}
