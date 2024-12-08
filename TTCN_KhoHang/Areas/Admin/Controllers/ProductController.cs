using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            var tmlist = _context.Products.OrderBy(m => m.product_id).ToList();
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

            var productDetail = _context.Products.FirstOrDefault(pd => pd.product_id == id);
            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        [HttpGet]
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

            var mn = _context.Products.Find(id);
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

            var dlprd = _context.Products.Find(id);
            if (dlprd == null)
            {
                return NotFound();
            }

            _context.Products.Remove(dlprd);
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

            var mnList = (from m in _context.Products
                select new SelectListItem()
                {
                    Text = m.name,
                    Value = m.product_id.ToString(),
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
        public IActionResult Create(Product mn)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }

        public IActionResult Edit(int? id)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var mn = _context.Products.Find(id);
            if (mn == null)
            {
                return NotFound();
            }

            var mnList = (from m in _context.Products
                select new SelectListItem()
                {
                    Text = m.name,
                    Value = m.product_id.ToString(),
                }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = String.Empty
            });
            ViewBag.mnList = mnList;

            return View(mn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product mn)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Update(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }
        public IActionResult LowStockAndExpiryAlert()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            // Ngưỡng số lượng và thời gian hết hạn để cảnh báo
            int lowStockThreshold = 10; // Số lượng nhỏ hơn 10 thì cảnh báo
            int expiryThresholdDays = 30; // Gần hết hạn trong vòng 30 ngày

            // Lấy ngày hiện tại
            var currentDate = DateTime.Now;

            // Tìm các sản phẩm cần cảnh báo
            var alerts = _context.Products
                .Where(p => p.quantity < lowStockThreshold ||
                            (p.exp.HasValue && p.exp.Value <= currentDate.AddDays(expiryThresholdDays)))
                .OrderBy(p => p.exp)
                .ToList();

            return View(alerts);
        }
    }
}