using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Models;
using Core;
using _DataAccess.Repository.IRepository;
using _DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Core.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(o => o.Password.RequireNonAlphanumeric = false)
        .AddEntityFrameworkStores<Context>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = @"/Identity/Account/Logins";
    options.LogoutPath = @"/User/Home/Index";
    options.AccessDeniedPath = @"/Identity/Account/AccessDenied";
});
builder.Services.AddSession();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.MapControllerRoute("route", "emp/{id:int}", new
{
    area = "User",
    controller = "Friends",
    action = "Index"
});

app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chat/{id}");


app.Run();
