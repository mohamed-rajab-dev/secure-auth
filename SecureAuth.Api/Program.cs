using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureAuth.Api.Authorization;
using SecureAuth.Api.Filters;
using SecureAuth.Api.Middlewares;
using SecureAuth.Application;
using SecureAuth.Infrastructure;

namespace SecureAuth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.AddService<ValidationFilter>();
            });

            builder.Services.AddScoped<ValidationFilter>();

            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            builder.Services.AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallback(() =>
            {
                return Results.NotFound(new
                {
                    success = false,
                    message = "Endpoint not found."
                });
            });

            app.Run();
        }
    }
}