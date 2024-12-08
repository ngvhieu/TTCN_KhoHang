using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductCategoryController : Controller
	{
		private readonly DataContext _context;

		public ProductCategoryController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}
			var tmlist = _context.ProductCategorys.OrderBy(m => m.category_id).ToList();
			return View(tmlist);
		}
		public IActionResult Details(int? id)
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}

			if (id == null || id == 0)
			{
				return NotFound();
			}
			var productDetail = _context.ProductCategorys.FirstOrDefault(pd => pd.category_id == id);
			if (productDetail == null)
			{
				return NotFound();
			}
			return View(productDetail);
		}
		
		public IActionResult Delete(int? id)
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var mn = _context.ProductCategorys.Find(id);
			if (mn == null)
			{
				return NotFound();
			}
			return View(mn);
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}

			var dele = _context.ProductCategorys.Find(id);
			if (dele == null)
			{
				return NotFound();
			}
			_context.ProductCategorys.Remove(dele);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Create()
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}

			var mnList = (from m in _context.ProductCategorys
						  select new SelectListItem()
						  {
							  Text = m.categoryname,
							  Value = m.category_id.ToString(),
						  }).ToList();
			mnList.Insert(0, new SelectListItem()
			{
				Text = "----Select----",
				Value = "0"
			});
			ViewBag.mnList = mnList;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ProductCategory mn)
		{
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}

			if (ModelState.IsValid)
			{
				_context.ProductCategorys.Add(mn);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(mn);
		}
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var mn = _context.ProductCategorys.Find(id);
			if (mn == null)
			{
				return NotFound();
			}
			var mnList = (from m in _context.ProductCategorys
						  select new SelectListItem()
						  {
							  Text = m.categoryname,
							  Value = m.category_id.ToString(),
						  }).ToList();
			mnList.Insert(0, new SelectListItem()
			{
				Text = "----Select----",
				Value = string.Empty
			});
			ViewBag.mnList = mnList;
			return View(mn);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ProductCategory us)
		{
			if (ModelState.IsValid)
			{
				_context.ProductCategorys.Update(us);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(us);
		}

	}
}
