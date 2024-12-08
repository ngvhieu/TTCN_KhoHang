
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TTCN_KhoHang.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int supplier_id { get; set; }
        public string supplier_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string tax { get; set; }
    }
}

