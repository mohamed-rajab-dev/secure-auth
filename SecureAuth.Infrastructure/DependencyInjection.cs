using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureAuth.Domain.Entities;
using SecureAuth.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                migration => migration.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)

                );

            });

            services.AddIdentity<User, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            return services;
        }

    }
}
