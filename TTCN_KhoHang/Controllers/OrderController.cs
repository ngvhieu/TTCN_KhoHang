using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TTCN_KhoHang.Models;
using TTCN_KhoHang.Utilities;
using TTCN_KhoHang.ViewModels;

namespace TTCN_KhoHang.Controllers
{
	public class OrderUserController : Controller
	{
		private readonly DataContext _context;

		public OrderUserController(DataContext context)
		{
			_context = context;
		}

		public IActionResult Index(int userId)
		{
			// Lấy danh sách đơn hàng của người dùng
			userId = Functions._UserID;
			var orders = (from o in _context.Order
						  join od in _context.OrderDetail on o.order_id equals od.order_id
						  join p in _context.Products on od.product_id equals p.product_id
						  join z in _context.users on o.user_id equals z.userid
                          where o.user_id == userId
						  select new UserOrderView
						  {
                              user_name = z.username,
                              order_id = o.order_id,
							  order_date = o.order_date,
							  order_status = o.order_status,
							  product_id = p.product_id,
							  product_name = p.name,
							  quantity = od.quantity,
							  unit_price = od.unit_price,
							  total_price = od.quantity * od.unit_price,
							  total_amount = _context.OrderDetail
								  .Where(d => d.order_id == o.order_id)
								  .Sum(d => d.quantity * d.unit_price)
						  }).ToList();


			return View(orders);
		}
	}
}
