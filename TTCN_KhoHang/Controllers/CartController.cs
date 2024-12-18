using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCN_KhoHang.Utilities;
using TTCN_KhoHang.Models;
using System.Linq;

namespace TTCN_KhoHang.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _context;

		public CartController(DataContext context)
		{
			_context = context;
		}
		private Cart GetOrCreateCart(int userId)
		{
			// Tìm giỏ hàng của người dùng
			var cart = _context.Carts.FirstOrDefault(c => c.user_id == userId);

			if (cart == null)
			{
				// Nếu chưa có giỏ hàng, tạo mới
				cart = new Cart
				{
					user_id = userId,
					CartItems = new List<CartItem>()
				};

				_context.Carts.Add(cart);
				_context.SaveChanges();
			}
			else
			{
				// Nếu đã có giỏ hàng, truy vấn danh sách CartItems
				cart.CartItems = _context.CartItems
					.Where(ci => ci.cart_id == cart.cart_id)
					.ToList();
				// Truy vấn thông tin sản phẩm cho từng CartItem
				foreach (var cartItem in cart.CartItems)
				{
					cartItem.Product = _context.Products.FirstOrDefault(p => p.product_id == cartItem.product_id);
				}
			}

			return cart;
		}
		public IActionResult Index()
		{
			int userid = Functions._UserID;
			var cart = GetOrCreateCart(userid);
			return View(cart);
		}

		public IActionResult AddToCart(int productId)
		{
			int userId = Functions._UserID;
			var cart = _context.Carts.FirstOrDefault(c => c.user_id == userId);

			if (cart == null)
			{
				// Tạo mới cart nếu chưa tồn tại
				cart = new Cart
				{
					user_id = userId
				};
				_context.Carts.Add(cart);
				_context.SaveChanges(); // Lưu để có cart_id
			}

			var product = _context.Products.FirstOrDefault(p => p.product_id == productId);

			if (product != null)
			{
				var cartItem = _context.CartItems.FirstOrDefault(ci => ci.cart_id == cart.cart_id && ci.product_id == productId);
				if (cartItem == null)
				{
					cartItem = new CartItem
					{
						cart_id = cart.cart_id,
						product_id = productId,
						quantity = 1
					};
					_context.CartItems.Add(cartItem);
				}
				else
				{
					cartItem.quantity++;
					_context.CartItems.Update(cartItem);
				}

				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult UpdateCart(int cartItemId, int quantity)
		{
			var cartItem = _context.CartItems.Find(cartItemId);

			if (cartItem != null)
			{
				cartItem.quantity = quantity;
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		public IActionResult RemoveFromCart(int cartItemId)
		{
			var cartItem = _context.CartItems.Find(cartItemId);

			if (cartItem != null)
			{
				_context.CartItems.Remove(cartItem);
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		public IActionResult Checkout()
		{
			int userid = Functions._UserID;
			var cart = GetOrCreateCart(userid);
			return View(cart);
		}

		[HttpPost]
		public IActionResult ProcessOrder()
		{
			int userid = Functions._UserID;
			var cart = GetOrCreateCart(userid);

			if (cart.CartItems.Any())
			{
				try
				{
					var order = new Order
					{
						user_id = userid,
						order_date = DateTime.Now,
						total_amount = cart.CartItems.Sum(ci => ci.quantity * ci.Product.price_out),
						order_status = "Pending"
					};
					_context.Add(order);
					_context.SaveChanges();
					// Tạo chi tiết đơn hàng
					foreach (var item in cart.CartItems)
					{
						var orderDetail = new OrderDetail
						{
							order_id = order.order_id,
							product_id = item.product_id,
							quantity = item.quantity,
							unit_price = item.Product.price_out,
							total_price = item.quantity * item.Product.price_out
						};
						_context.OrderDetail.Add(orderDetail);
						var product = _context.Products.Find(item.product_id);
						product.quantity -= item.quantity;
					}
					_context.SaveChanges();

					// Xóa giỏ hàng
					_context.CartItems.RemoveRange(cart.CartItems);
					_context.Carts.Remove(cart);
					_context.SaveChanges();

					TempData["Success"] = "Đặt hàng thành công!";
				}
				catch (Exception e)
				{
					TempData["Error"] = "Có lỗi xảy ra khi đặt hàng.";
				}
			}
			return RedirectToAction("Index", "ProductUser");
		}

	}
}
