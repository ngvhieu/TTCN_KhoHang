using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TTCN_KhoHang.Models
{
    [Table("ImportDetail")]
    public class ImportDetail
    {
        [Key]
        public int import_detail_id { get; set; }
        public int import_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_price { get; set; }
    }
    public class ImportDetailView:ImportDetail
    {
        public string name { get; set; }
    }
}

