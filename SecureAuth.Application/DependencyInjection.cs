using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SecureAuth.Application.Interfaces.Services;
using SecureAuth.Application.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecureAuth.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMapster();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            services.AddScoped<IAuthService,AuthServices>();

            return services;
        }
    }
}
