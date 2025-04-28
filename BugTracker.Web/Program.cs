using AspNetCore.Identity.Extensions;
using BugTracker.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. Core infrastructure (DbContext, Identity)
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Repositories, business services, and mapping
builder.Services
    .AddDataRepositories()
    .AddBusinessServices()
    .AddSqlLogging();

// 3. Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// 4. Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.ApplyMigrations();
    await app.SeedDataAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 5. Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Issue}/{action=Index}/{id?}");
app.MapRazorPages();


await app.RunAsync();