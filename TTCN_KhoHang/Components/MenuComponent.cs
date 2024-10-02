using Microsoft.AspNetCore.Mvc;
using TTCN_KhoHang.Models;
namespace TTCN_KhoHang.Components
{
    [ViewComponent(Name = "MenuView")]
    public class MenuComponent : ViewComponent
    {
        private DataContext _context;
        public MenuComponent(DataContext context) 
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listmenu = (from m in _context.Menu
                            where (m.IsActive == true) 
                            select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listmenu));
        }
    }
}
