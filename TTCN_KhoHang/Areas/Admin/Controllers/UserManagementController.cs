using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")] 
    public class UserManagementController : Controller
    {
        private readonly DataContext _context;
        public UserManagementController(DataContext context)
        {
            _context = context;
        }


		public IActionResult Index()
		{
			if (!Functions.IsLogin())
				return RedirectToAction("Index", "Login");

			var mnList = _context.users.OrderBy(m => m.userid).ToList();
			return View(mnList);
		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var mn = _context.users.Find(id);
			if (mn == null)
			{
				return NotFound();
			}
			return View(mn);
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
			var dele = _context.users.Find(id);
			if (dele == null)
			{
				return NotFound();
			}
			_context.users.Remove(dele);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Create()
		{
			var mnList = (from m in _context.users
						  select new SelectListItem()
						  {
							  Text = m.username,
							  Value = m.userid.ToString(),
						  }).ToList();
			mnList.Insert(0, new SelectListItem()
			{
				Text = "---Select----",
				Value = "0"
			});
			ViewBag.mnList = mnList;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(User us)
		{
			if (ModelState.IsValid)
			{
				us.password = Utilities.Functions.MD5Password(us.password);
				_context.users.Add(us);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(us);
		}
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var mn = _context.users.Find(id);
			if (mn == null)
			{
				return NotFound();
			}
			var mnList = (from m in _context.users
						  select new SelectListItem()
						  {
							  Text = m.username,
							  Value = m.userid.ToString(),
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
		public IActionResult Edit(User us)
		{
			if (ModelState.IsValid)
			{
				_context.users.Update(us);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(us);
		}
	}
}
