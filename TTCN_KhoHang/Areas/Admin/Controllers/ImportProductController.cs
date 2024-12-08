using TTCN_KhoHang.Models;
using TTCN_KhoHang.ViewModels;
using TTCN_KhoHang.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TTCN_KhoHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImportProductController : Controller
	{
		private readonly DataContext _context;

		public ImportProductController(DataContext context)
		{
			_context = context;
		}
		public ActionResult Index()
		{
			var import = (from a in _context.ImportProducts
						  join b in _context.users on a.user_id equals b.userid
						  join c in _context.Suppliers on a.supplier_id equals c.supplier_id
						  join d in _context.Warehouses on a.warehouse_id equals d.warehouse_id
						  join e in _context.ImportDetails on a.import_id equals e.import_id
						  join f in _context.Products on e.product_id equals f.product_id
						  select new ImportView
						  {
							  import_id = a.import_id,
							  import_date = a.import_date,
							  total_amount = a.total_amount,
							  supplier_name = c.supplier_name,
							  warehouse_name = d.warehousename,
							  user_name = b.username,
							  status = a.status,
							  product_id = f.product_id,
							  name = f.name,
							  quantity = e.quantity,
							  unit_price = e.unit_price,
							  total_price = e.total_price,
						  }).ToList();
			return View(import);
		}
		public ActionResult Create()
		{
			var viewModel = new ImportView
			{
				Suppliers = _context.Suppliers.ToList(),
				Warehouses = _context.Warehouses.ToList(),
				Products = _context.Products.ToList()
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> Create(ImportProduct model, List<ImportDetail> ImportDetails)
		{
			try
			{
				// Thiết lập thông tin cho ImportProduct
				model.import_date = DateTime.Now;
				model.user_id = Functions._UserID;
				model.status = "Pending";

				// Lưu ImportProduct vào cơ sở dữ liệu
				_context.ImportProducts.Add(model);
				_context.SaveChanges();
				// Thêm từng ImportDetail và cập nhật số lượng sản phẩm
				foreach (var detail in ImportDetails)
				{
					detail.import_id = model.import_id;
					_context.Add(detail);

					// Cập nhật số lượng sản phẩm trong bảng Products
					var product = _context.Products.FirstOrDefault( m=> m.product_id == detail.product_id);
					if (product != null)
					{
						product.quantity += detail.quantity;
						_context.Update(product);
					}
				}

				await _context.SaveChangesAsync();

				TempData["Success"] = "Tạo phiếu nhập hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// Log lỗi và hiển thị thông báo lỗi
				TempData["Error"] = "Có lỗi xảy ra khi tạo phiếu nhập hàng. Vui lòng thử lại!";

				// Tạo lại viewModel và trả về View
				var viewModel = new ImportView
				{
					Suppliers = _context.Suppliers.ToList(),
					Warehouses = _context.Warehouses.ToList(),
					Products = _context.Products.ToList()
				};
				return View(viewModel);
			}
		}
		[HttpGet]
		public IActionResult Edit(int id)
		{
			// Lấy thông tin ImportProduct
			var importProduct = _context.ImportProducts.Find(id);

			if (importProduct == null)
			{
				return NotFound();
			}

			// Lấy danh sách các ImportDetails liên quan
			var importDetails = _context.ImportDetails.Where(d => d.import_id == id).ToList();

			// Tạo ViewModel chứa thông tin ImportProduct và các lựa chọn từ các bảng khác
			var viewModel = new ImportView
			{
				import_id = importProduct.import_id,
				import_date = importProduct.import_date,
				total_amount = importProduct.total_amount,
				supplier_id = importProduct.supplier_id,
				warehouse_id = importProduct.warehouse_id,
				user_id = importProduct.user_id,
				status = importProduct.status,
				Suppliers = _context.Suppliers.ToList(),
				Warehouses = _context.Warehouses.ToList(),
				Products = _context.Products.ToList(),
				ImportDetails = importDetails

			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("/ImportProduct/Edit/{id:int}")]
		public ActionResult Edit(int id, ImportView viewModel)
		{
			try
			{
				var importProduct = _context.ImportProducts.FirstOrDefault(m => m.import_id == viewModel.import_id);
				if (importProduct == null)
				{
					return NotFound();
				}

				// Cập nhật thông tin ImportProduct
				importProduct.supplier_id = viewModel.supplier_id;
				importProduct.warehouse_id = viewModel.warehouse_id;
				importProduct.status = viewModel.status;

				// Thêm các chi tiết đã chỉnh sửa

				_context.SaveChanges();

				TempData["Success"] = "Cập nhật phiếu nhập hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				TempData["Error"] = "Có lỗi xảy ra khi cập nhật phiếu nhập hàng. Vui lòng thử lại!";
				return View(viewModel);
			}
		}
		[HttpPost]
		[IgnoreAntiforgeryToken]
		[Route("/ImportProduct/Edit/importdetail/{import_id:int}")]
		public async Task<IActionResult> updateProduct(int import_id, ImportDetail importDetail)
		{
			var importProduct = _context.ImportProducts.FirstOrDefault(m => m.import_id == import_id);
			if (importProduct == null)
			{
				return Redirect($"/ImportProduct/Edit/{import_id}");
			}

			var oldImportDetail = _context.ImportDetails.FirstOrDefault(d => d.import_id == importDetail.import_id && d.product_id == importDetail.product_id && d.import_detail_id == importDetail.import_detail_id);
			if (oldImportDetail == null)
			{
				return Redirect($"/ImportProduct/Edit/{import_id}");
			}

			_context.ChangeTracker.Clear(); // xoá theo dõi của 2 truy vấn trên
			importProduct.total_amount = importProduct.total_amount - oldImportDetail.quantity * oldImportDetail.unit_price + importDetail.quantity * importDetail.unit_price;
			_context.Update(importProduct);
			// Cập nhật số lượng sản phẩm
			var product = _context.Products.FirstOrDefault(m => m.product_id == importDetail.product_id);
			if (product == null)
			{
				return Redirect($"/ImportProduct/Edit/{import_id}");
			}

			product.quantity = product.quantity - oldImportDetail.quantity + importDetail.quantity;
			if (product.quantity < 0)
			{
				//quá số lượng
				return Redirect($"/ImportProduct/Edit/{import_id}");
			}

			importDetail.total_price = importDetail.quantity * importDetail.unit_price;
			_context.ImportDetails.Update(importDetail);
			_context.Update(product);
			await _context.SaveChangesAsync();
			return Redirect($"/ImportProduct/Edit/{import_id}");
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			try
			{
				var importProduct = _context.ImportProducts.Find(id);
				if (importProduct == null)
				{
					return NotFound();
				}
				// Remove related ImportDetails
				var importDetails = _context.ImportDetails.Where(d => d.import_id == id).ToList();
				foreach (var detail in importDetails)
				{
					var product = _context.Products.Find(detail.product_id);
					if (product != null)
					{
						product.quantity -= detail.quantity; // Adjust product quantity
					}
					_context.ImportDetails.Remove(detail);
				}
				_context.ImportProducts.Remove(importProduct);
				_context.SaveChanges(); // Save changes synchronously

				TempData["Success"] = "Xóa phiếu nhập hàng thành công!";
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				TempData["Error"] = "Có lỗi xảy ra khi xóa phiếu nhập hàng. Vui lòng thử lại!";
				return RedirectToAction("Index");
			}
		}
        [HttpGet]
        public JsonResult GetImportStats()
        {
            var stats = _context.ImportProducts
                .GroupBy(x => x.import_date.Month)
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


