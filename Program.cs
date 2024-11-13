using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MakebA_Final.Data;

var builder = WebApplication.CreateBuilder(args);

// การเชื่อมต่อฐานข้อมูล
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// การตั้งค่าระบบ Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// การตั้งค่าระบบ Authentication (Cookie Authentication)
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Login"; // เส้นทางหน้า Login
        options.AccessDeniedPath = "/AccessDenied"; // เส้นทางหน้า Access Denied
    });

// เพิ่ม Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// การตั้งค่าระบบ Middleware สำหรับ routing, authentication, และ authorization
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// เปิดการยืนยันตัวตนและการอนุญาต
app.UseAuthentication();
app.UseAuthorization();

// ตั้งค่าเส้นทาง Razor Pages
app.MapRazorPages();

app.Run();
