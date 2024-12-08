using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        
        private readonly DataContext _context;
        public CustomersController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            var customers = _context.Customers.OrderBy(c => c.customer_id).ToList();
            return View(customers);
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

            var customer = _context.Customers.FirstOrDefault(c => c.customer_id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
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

            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
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

            var customerList = (from c in _context.Customers
                                select new SelectListItem
                                {
                                    Text = c.customer_name,
                                    Value = c.customer_id.ToString()
                                }).ToList();

            customerList.Insert(0, new SelectListItem
            {
                Text = "----Select----",
                Value = "0"
            });

            ViewBag.CustomerList = customerList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer mn)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Customers.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mn);
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateAPI([FromBody]Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return Json(new { customer = customer });
            }

            return Json(new { error = "Có lỗi xảy ra khi thêm khách hàng mới." });
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

            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            var customerList = (from c in _context.Customers
                                select new SelectListItem
                                {
                                    Text = c.customer_name,
                                    Value = c.customer_id.ToString()
                                }).ToList();

            customerList.Insert(0, new SelectListItem
            {
                Text = "----Select----",
                Value = string.Empty
            });

            ViewBag.CustomerList = customerList;
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer customer)
        {
            if (!Functions.IsLogin())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Customers.Update(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
    }
}