using CraftyShop.Data;
using CraftyShop.Models;
using Microsoft.AspNetCore.Identity;

namespace CraftyShop.Extensions
{
    public static class SeedDataExtension
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedData.Initialize(scope.ServiceProvider, userManager, roleManager).Wait();
        }
    }
}
