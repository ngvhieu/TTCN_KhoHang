using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TTCN_KhoHang.Models
{   
    [Table("ImportProduct")]
    public class ImportProduct
    {   
        [Key]
        public int import_id { get; set; }
        public DateTime import_date { get; set; }
        public decimal total_amount { get; set; }
        public string note { get; set; }
        public int supplier_id { get; set; }
        public int user_id { get; set; }
        public string status { get; set; }
        public int warehouse_id { get; set; }
    }    
}
