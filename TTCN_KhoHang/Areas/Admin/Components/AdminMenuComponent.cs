using Microsoft.AspNetCore.Mvc;
using TTCN_KhoHang.Models;
namespace TTCN_KhoHang.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminMenuView")]
    public class AdminMenuComponent : ViewComponent
    {
        private DataContext _context;
        public AdminMenuComponent(DataContext context) 
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listmenu = (from m in _context.AdminMenu
                            where (m.IsActive == true) 
                            select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listmenu));
        }
    }
}
