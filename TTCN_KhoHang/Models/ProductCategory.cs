using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace TTCN_KhoHang.Models
{
	[Table("ProductCategory")]
	public class ProductCategory
	{
		[Key]
		public int category_id { get; set; }
		public string categoryname { get; set; }
		public string note { get; set; }
		
	}
}
