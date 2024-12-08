using Microsoft.AspNetCore.Mvc;
using TTCN_KhoHang.Utilities;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
			if (!Functions.IsLogin())
			{
				return RedirectToAction("Index", "Login");
			}
			else
			{
				const int adminRoleId = 2; // Khai báo hằng số cho vai trò "Admin"
				if (Functions._Role != adminRoleId)
				{
					return RedirectToAction("Index", "ErrorRole");
				}
			}
			return View();
			
		}
    }
}
