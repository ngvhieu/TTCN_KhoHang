using Microsoft.AspNetCore.Mvc;
using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;

namespace TTCN_KhoHang.Controllers
{
    [Route("~/register")]
    public class RegisterController : Controller
    {
        private readonly DataContext _context;

        public RegisterController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User user)
        {
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra xem email đã tồn tại chưa
            var check = _context.users.FirstOrDefault(m => m.email == user.email);
            if (check != null)
            {
                ViewBag.MessageEmail = "Duplicate Email!";
                return View();
            }

            // Mã hóa mật khẩu
            user.password = Functions.MD5Password(user.password);

            // Gán vai trò mặc định
            user.role = 1;

            // Thêm người dùng mới vào cơ sở dữ liệu
            _context.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }
    }
}
