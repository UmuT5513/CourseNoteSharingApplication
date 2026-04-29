using Microsoft.EntityFrameworkCore;

using CourseNoteSharingSystem.Data;

using CourseNoteSharingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User, Role>( opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 1;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Lockout.MaxFailedAccessAttempts = 5;
}).AddEntityFrameworkStores<CourseNoteSharingSystemContext>();

// veritabanı servisini ekle
builder.Services.AddDbContext<CourseNoteSharingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon") ?? throw new InvalidOperationException("Connection string 'CourseNoteSharingSystemContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<CourseNoteSharingSystemContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")); }); // SqlCon appsettings.json dosyasındaki connection string'in adı

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
