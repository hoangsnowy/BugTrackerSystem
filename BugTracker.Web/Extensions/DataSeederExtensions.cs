using BugTracker.Data.Models;
using BugTracker.Web.DesignTimeFactories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BugTracker.Web.Extensions
{
    public static class DataSeederExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = services.GetRequiredService<UserManager<User>>();
            await SeedData.InitializeAsync(roleMgr, userMgr);
        }
    }
}
