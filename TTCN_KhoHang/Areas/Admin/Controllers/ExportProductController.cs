using TTCN_KhoHang.Models;
using TTCN_KhoHang.ViewModels;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExportProductController : Controller
	{
		private readonly DataContext _context;

		public ExportProductController(DataContext context)
		{
			_context = context;
		}

		public ActionResult Index()
		{
			var export = (from a in _context.ExportProducts
						  join b in _context.users on a.user_id equals b.userid
						  join c in _context.Customers on a.customer_id equals c.customer_id
						  join d in _context.Warehouses on a.warehouse_id equals d.warehouse_id
						  join e in _context.ExportDetails on a.export_id equals e.export_id
						  join f in _context.Products on e.product_id equals f.product_id
						  select new ExportView
						  {
							  export_id = a.export_id,
							  export_date = a.export_date,
							  total_amount = a.total_amount,
							  customer_name = c.customer_name,
							  warehouse_name = d.warehousename,
							  user_name = b.username,
							  status = a.status,
							  product_id = f.product_id,
							  name = f.name,
							  quantity = e.quantity,
							  unit_price = e.unit_price,
							  total_price = e.total_price,
						  }).ToList();
			return View(export);
		}

		public ActionResult Create()
		{
			var viewModel = new ExportView
			{
				Customers = _context.Customers.ToList(),
				Warehouses = _context.Warehouses.ToList(),
				Products = _context.Products.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> Create(ExportProduct model, List<ExportDetail> ExportDetails)
		{
			try
			{
				model.export_date = DateTime.Now;
				model.user_id = Functions._UserID;
				model.status = "Pending";

				_context.ExportProducts.Add(model);
				_context.SaveChanges();

				foreach (var detail in ExportDetails)
				{
					detail.export_id = model.export_id;

					// Kiểm tra số lượng tồn kho trước khi xuất
					var product = _context.Products.FirstOrDefault(m => m.product_id == detail.product_id);
					if (product == null || product.quantity < detail.quantity)
					{
						throw new Exception("Số lượng sản phẩm trong kho không đủ!");
					}

					_context.Add(detail);

					// Cập nhật số lượng sản phẩm (giảm đi)
					product.quantity -= detail.quantity;
					_context.Update(product);
				}

				await _context.SaveChangesAsync();

				TempData["Success"] = "Tạo phiếu xuất hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
				var viewModel = new ExportView
				{
					Customers = _context.Customers.ToList(),
					Warehouses = _context.Warehouses.ToList(),
					Products = _context.Products.ToList()
				};
				return View(viewModel);
			}
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var exportProduct = _context.ExportProducts.Find(id);
			if (exportProduct == null)
			{
				return NotFound();
			}

			var exportDetails = _context.ExportDetails.Where(d => d.export_id == id).ToList();

			var viewModel = new ExportView
			{
				export_id = exportProduct.export_id,
				export_date = exportProduct.export_date,
				total_amount = exportProduct.total_amount,
				customer_id = exportProduct.customer_id,
				warehouse_id = exportProduct.warehouse_id,
				user_id = exportProduct.user_id,
				status = exportProduct.status,
				Customers = _context.Customers.ToList(),
				Warehouses = _context.Warehouses.ToList(),
				Products = _context.Products.ToList(),
				ExportDetails = exportDetails
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("/ExportProduct/Edit/{id:int}")]
		public ActionResult Edit(int id, ExportView viewModel)
		{
			try
			{
				var exportProduct = _context.ExportProducts.FirstOrDefault(m => m.export_id == viewModel.export_id);
				if (exportProduct == null)
				{
					return NotFound();
				}

				exportProduct.customer_id = viewModel.customer_id;
				exportProduct.warehouse_id = viewModel.warehouse_id;
				exportProduct.status = viewModel.status;

				_context.SaveChanges();

				TempData["Success"] = "Cập nhật phiếu xuất hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				TempData["Error"] = "Có lỗi xảy ra khi cập nhật phiếu xuất hàng.";
				return View(viewModel);
			}
		}

		[HttpPost]
		[IgnoreAntiforgeryToken]
		[Route("/ExportProduct/Edit/exportdetail/{export_id:int}")]
		public async Task<IActionResult> updateProduct(int export_id, ExportDetail exportDetail)
		{
			var exportProduct = _context.ExportProducts.FirstOrDefault(m => m.export_id == export_id);
			if (exportProduct == null)
			{
				return Redirect($"/ExportProduct/Edit/{export_id}");
			}

			var oldExportDetail = _context.ExportDetails.FirstOrDefault(
				d => d.export_id == exportDetail.export_id &&
				d.product_id == exportDetail.product_id &&
				d.export_detail_id == exportDetail.export_detail_id
			);

			if (oldExportDetail == null)
			{
				return Redirect($"/ExportProduct/Edit/{export_id}");
			}

			_context.ChangeTracker.Clear();

			// Cập nhật tổng tiền của phiếu xuất
			exportProduct.total_amount = exportProduct.total_amount -
				oldExportDetail.quantity * oldExportDetail.unit_price +
				exportDetail.quantity * exportDetail.unit_price;

			_context.Update(exportProduct);

			// Cập nhật số lượng sản phẩm trong kho
			var product = _context.Products.FirstOrDefault(m => m.product_id == exportDetail.product_id);
			if (product == null)
			{
				return Redirect($"/ExportProduct/Edit/{export_id}");
			}

			// Hoàn trả số lượng cũ và trừ đi số lượng mới
			product.quantity = product.quantity + oldExportDetail.quantity - exportDetail.quantity;
			if (product.quantity < 0)
			{
				TempData["Error"] = "Số lượng sản phẩm trong kho không đủ!";
				return Redirect($"/ExportProduct/Edit/{export_id}");
			}

			exportDetail.total_price = exportDetail.quantity * exportDetail.unit_price;
			_context.ExportDetails.Update(exportDetail);
			_context.Update(product);
			await _context.SaveChangesAsync();

			return Redirect($"/ExportProduct/Edit/{export_id}");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			try
			{
				var exportProduct = _context.ExportProducts.Find(id);
				if (exportProduct == null)
				{
					return NotFound();
				}

				var exportDetails = _context.ExportDetails.Where(d => d.export_id == id).ToList();
				foreach (var detail in exportDetails)
				{
					var product = _context.Products.Find(detail.product_id);
					if (product != null)
					{
						// Hoàn trả số lượng sản phẩm vào kho
						product.quantity += detail.quantity;
						_context.Update(product);
					}
					_context.ExportDetails.Remove(detail);
				}

				_context.ExportProducts.Remove(exportProduct);
				_context.SaveChanges();

				TempData["Success"] = "Xóa phiếu xuất hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				TempData["Error"] = "Có lỗi xảy ra khi xóa phiếu xuất hàng.";
				return RedirectToAction("Index");
			}
		}
        public JsonResult GetExportStats()
        {
            var stats = _context.ExportProducts
                .GroupBy(x => x.export_date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalAmount = g.Sum(x => x.total_amount)
                }).ToList();

            return Json(stats);
        }
        public IActionResult Statistics()
        {
            return View();
        }
    }
}