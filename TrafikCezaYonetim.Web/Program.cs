using Microsoft.EntityFrameworkCore;
using TrafikCezaYonetim.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

// 1. Servisleri Konteynere Ekle (builder.Build()'den ÖNCE olmalı)
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Uygulamayı İnşa Et
var app = builder.Build();

// 3. HTTP İstek Hattını Yapılandır
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();