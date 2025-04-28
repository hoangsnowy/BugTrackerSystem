using System;
using System.Threading.Tasks;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BugTracker.Web.DesignTimeFactories
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            RoleManager<IdentityRole> roleMgr,
            UserManager<User> userMgr)
        {
            // 1) Khởi tạo roles
            var roles = new[] { "Tester", "Developer", "ProjectManager", "Admin" };
            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }

            // 2) Khởi tạo user mẫu và gán role
            async Task CreateUser(string userName, string email, string password, string role)
            {
                if (await userMgr.FindByNameAsync(userName) is null)
                {
                    var user = new User { UserName = userName, Email = email, EmailConfirmed = true, Login = userName };
                    var result = await userMgr.CreateAsync(user, password);
                    if (result.Succeeded)
                        await userMgr.AddToRoleAsync(user, role);
                }
            }

            await CreateUser("tester1", "tester1@example.com", "Password123!", "Tester");
            await CreateUser("dev1", "dev1@example.com", "Password123!", "Developer");
            await CreateUser("pm1", "pm1@example.com", "Password123!", "ProjectManager");
            await CreateUser("admin1", "admin1@example.com", "Password123!", "Admin");
            // Tài khoản dùng cho automation test
            await CreateUser("autotest1", "autotest1@example.com", "Password123!", "Tester");
        }
    }
}
