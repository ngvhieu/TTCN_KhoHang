using Microsoft.AspNetCore.Mvc;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
