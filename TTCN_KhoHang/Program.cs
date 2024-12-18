//    using Microsoft.EntityFrameworkCore;
//using TTCN_KhoHang.Models;

//var builder = WebApplication.CreateBuilder(args);
//var connection = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");




//app.Run();
using Microsoft.EntityFrameworkCore;
using TTCN_KhoHang.Models;

var builder = WebApplication.CreateBuilder(args);

// Kết nối cơ sở dữ liệu
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình Middleware cho các môi trường
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Định tuyến cần phải đặt trước Endpoints
app.UseAuthorization();

// Định nghĩa Endpoint
app.UseEndpoints(endpoints =>
{
    // Route cho Area
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // Route mặc định
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
