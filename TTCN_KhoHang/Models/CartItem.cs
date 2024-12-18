using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
namespace TTCN_KhoHang.Models
{
	[Table("CartItem")]
	[PrimaryKey(nameof(cart_item_id))]
	public class CartItem
	{
		[Key]
		public int cart_item_id { get; set; }
		[ForeignKey(nameof(Cart)),Required]
		public int cart_id { get; set; }
		[ForeignKey(nameof(Product)),Required]
		public int product_id { get; set; }
        public Cart? Cart { get; set; }
		public Product? Product { get; set; }
		public int quantity { get; set; }
	}
}
