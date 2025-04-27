using BugTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseLazyLoadingProxies();
  options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
  options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<EfCoreRepository<Issue, ApplicationDbContext>, EfCoreIssueRepository>();
builder.Services.AddScoped<EfCoreRepository<User, ApplicationDbContext>, EfCoreUserRepository>();
builder.Services.AddScoped<EfCoreRepository<Status, ApplicationDbContext>, EfCoreStatusRepository>();
builder.Services.AddScoped<EfCoreRepository<Priority, ApplicationDbContext>, EfCorePriorityRepository>();

builder.Services.AddLogging(loggingBuilder =>
{
  // Output SQL queries
  loggingBuilder.AddConsole().AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "Identity/{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();