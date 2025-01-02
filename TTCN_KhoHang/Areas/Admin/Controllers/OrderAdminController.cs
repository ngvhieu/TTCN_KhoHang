
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCN_KhoHang.Models;
using TTCN_KhoHang.ViewModels;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderAdminController : Controller
    {
        private readonly DataContext _context;

        public OrderAdminController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = (from o in _context.Order
                          join u in _context.users on o.user_id equals u.userid
                          join od in _context.OrderDetail on o.order_id equals od.order_id
                          join p in _context.Products on od.product_id equals p.product_id
                          select new OrderView
                          {
                              customer_name = u.username,
                              order_id = o.order_id,
                              order_date = o.order_date,
                              user_name = u.username,
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

        [HttpPost]
        public IActionResult Approve(int orderId)
        {
            var order = _context.Order.FirstOrDefault(o => o.order_id == orderId);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng cần phê duyệt!";
                return RedirectToAction("Index");
            }

            var user = _context.users.FirstOrDefault(u => u.userid == order.user_id);

            var orderDetails = _context.OrderDetail
                .Where(od => od.order_id == order.order_id)
                .ToList();
            if (!orderDetails.Any())
            {
                TempData["Error"] = "Không tìm thấy chi tiết đơn hàng!";
                return RedirectToAction("Index");
            }

            var productDetails = _context.Products
                .Where(p => orderDetails.Select(od => od.product_id).Contains(p.product_id))
                .ToDictionary(p => p.product_id, p => p);

            var exportView = new ExportView
            {
                export_date = DateTime.Now,
                total_amount = orderDetails.Sum(od => od.quantity * od.unit_price),
                customer_id = order.user_id,
                warehouse_id = 1,
                user_id = order.user_id,
                status = "Approved",
                customer_name = user?.username ?? "Unknown",
                user_name = user?.username ?? "Unknown",
                ExportDetails = orderDetails.Select(od => new ExportDetail
                {
                    product_id = od.product_id,
                    quantity = od.quantity,
                    unit_price = od.unit_price,
                    total_price = od.quantity * od.unit_price
                }).ToList(),
                Customers = _context.Customers.ToList(),
                Warehouses = _context.Warehouses.ToList(),
                Products = _context.Products.ToList()
            };
            var exportProduct = new ExportProduct
            {
                export_date = exportView.export_date,
                user_id = exportView.user_id,
                warehouse_id = exportView.warehouse_id,
                customer_id = exportView.customer_id,
                total_amount = exportView.total_amount,
                status = exportView.status
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.ExportProducts.Add(exportProduct);
                    _context.SaveChanges();
                    foreach (var detail in orderDetails)
                    {
                        var exportDetail = new ExportDetail
                        {
                            export_id = exportProduct.export_id,
                            product_id = detail.product_id,
                            quantity = detail.quantity,
                            unit_price = detail.unit_price,
                            total_price = detail.quantity * detail.unit_price
                        };
                        _context.ExportDetails.Add(exportDetail);

                        var product = _context.Products.Find(detail.product_id);
                        if (product != null)
                        {
                            product.quantity -= detail.quantity;
                            _context.Products.Update(product);
                        }
                    }
                    order.order_status = "Approved";
                    _context.Order.Update(order);
                    _context.SaveChanges();
                    transaction.Commit();

                    TempData["Success"] = "Đơn hàng đã được phê duyệt thành công!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var order = _context.Order.Find(id);

            if (order != null && order.order_status == "Pending")
            {
				var orderDetails = _context.OrderDetail
			.Where(od => od.order_id == order.order_id)
			.ToList();

				if (orderDetails.Any())
				{
					foreach (var detail in orderDetails)
					{
						var product = _context.Products.Find(detail.product_id);
						if (product != null)
						{
							product.quantity += detail.quantity;
							_context.Products.Update(product);
						}
					}
				}
				order.order_status = "Rejected";
				_context.SaveChanges();
				TempData["Success"] = "Đơn hàng đã bị từ chối và số lượng sản phẩm đã được cộng lại.";
			}
			else
            {
                TempData["Error"] = "Không thể từ chối đơn hàng.";
            }

            return RedirectToAction("Index");
        }
    }
}