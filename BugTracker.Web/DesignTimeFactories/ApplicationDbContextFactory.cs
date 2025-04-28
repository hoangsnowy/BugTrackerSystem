using System;
using System.IO;
using BugTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Web.DesignTimeFactories
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Lấy đường dẫn tới thư mục chứa appsettings.json của Web project
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "BugTracker.Web"));

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connStr = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found.");

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
