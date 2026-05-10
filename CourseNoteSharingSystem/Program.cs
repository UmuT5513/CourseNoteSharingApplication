using Microsoft.EntityFrameworkCore;

using CourseNoteSharingSystem.Data;

using CourseNoteSharingSystem.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User, Role>( opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 1;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;

})
    .AddEntityFrameworkStores<CourseNoteSharingSystemContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Home/SignIn");
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "CNSSAuthCookie";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});

// dosya yükleme ayarları
// Dosya boyutu limiti (örn: 50MB)
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 52428800; // 50 MB (byte cinsinden)
});

// Kestrel için (IIS dışında çalışıyorsa)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50 MB
});


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
app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SignIn}/{id?}")
    .WithStaticAssets();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    
    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new Role { Name = "Admin", isUpdated = false });
    }
    
    // Create User role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new Role { Name = "User", isUpdated = false });
    }
    
    // Create default admin user
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "admin",
            Email = "admin@admin.com",
            birthDate = new DateTime(1990, 1, 1)
        };
        var result = await userManager.CreateAsync(adminUser, "admin123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.Run();
