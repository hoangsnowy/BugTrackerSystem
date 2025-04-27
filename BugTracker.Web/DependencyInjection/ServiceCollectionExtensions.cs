using BugTracker.Business.Services;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BugTracker.Web.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            var connectionString = config.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("DefaultConnection not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddSqlLogging(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.AddConsole()
                       .AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
            });
            return services;
        }

        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped<GenericRepository<Issue, ApplicationDbContext>, IssueRepository>();
            services.AddScoped<GenericRepository<User, ApplicationDbContext>, UserRepository>();
            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
