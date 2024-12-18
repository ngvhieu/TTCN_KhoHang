using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace TTCN_KhoHang.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public decimal? price_in { get; set; }
        public decimal? price_out { get; set; }
        public int? quantity { get; set; }
        public int? category_id { get; set; }
        public DateTime? mfg { get; set; }
        public DateTime? exp { get; set; }
        public string? location { get; set; }
        public string? image {  get; set; }
    }
}
