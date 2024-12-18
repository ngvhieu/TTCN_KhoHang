using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using TTCN_KhoHang.Models;
using X.PagedList.Extensions;

namespace TTCN_KhoHang.Controllers
{
	public class ProductUserController : Controller
	{
		private readonly DataContext _context;

		public ProductUserController(DataContext context)
		{
			_context = context;
		}

		public IActionResult Index(string searchQuery, int? page)
		{
			var products = _context.Products.AsQueryable();

			// Lọc sản phẩm có price_out không phải null
			products = products.Where(p => p.price_out != null);

			if (!string.IsNullOrEmpty(searchQuery))
			{
				products = products.Where(p => p.name.Contains(searchQuery));
			}

			products = products.OrderBy(p => p.name);
			int pageSize = 8;
			int pageNumber = page ?? 1;

			return View(products.ToPagedList(pageNumber, pageSize));
		}
	}
}
