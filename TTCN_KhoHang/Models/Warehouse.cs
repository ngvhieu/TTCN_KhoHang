using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace TTCN_KhoHang.Models
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int warehouse_id { get; set; }
        public string warehousename { get; set; }
        public string location { get; set; }
       
    }
}
