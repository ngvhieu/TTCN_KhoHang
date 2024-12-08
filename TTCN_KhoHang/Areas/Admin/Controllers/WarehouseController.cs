using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehouseController : Controller
    {

        private readonly DataContext _context;
        public WarehouseController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }
            var tmlist = _context.Warehouses.OrderBy(m => m.warehouse_id).ToList();
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
            var productDetail = _context.Warehouses.FirstOrDefault(pd => pd.warehouse_id == id);
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
            var mn = _context.Warehouses.Find(id);
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

            var dlprd = _context.Warehouses.Find(id); if (dlprd == null)
            {
                return NotFound();
            }
            _context.Warehouses.Remove(dlprd);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("/Create/{id}")]
        [HttpGet]
        public IActionResult Create()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            var mnList = (from m in _context.Warehouses
                          select new SelectListItem()
                          {
                              Text = m.warehousename,
                              Value = m.warehouse_id.ToString(),
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
        public IActionResult Create(Warehouse mn)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Warehouses.Add(mn);
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
            var mn = _context.Warehouses.Find(id);
            if (mn == null)
            {
                return NotFound();
            }

            var mnList = (from m in _context.Warehouses
                          select new SelectListItem()
                          {
                              Text = m.warehousename,
                              Value = m.warehouse_id.ToString(),
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
        public IActionResult Edit(Warehouse mn)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {

                _context.Warehouses.Update(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mn);
        }

    }
}
