using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace TTCN_KhoHang.Models
{
	[Table("Cart")]
	public class Cart
	{
		[Key]
		public int cart_id { get; set; }
		public int user_id { get; set; }
		public List<CartItem> CartItems { get; set; }
	}
}
