using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTCN_KhoHang.Models
{
	[Table("User")]
	public class User
	{
		[Key]
		public int userid { get; set; }
		public string username { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public string? phone {  get; set; }
		//public DateTime? create_at { get; set; }
		public string? avatar {  get; set; }
		public bool? is_active { get; set; }
		public int? role {  get; set; }
	}
}
