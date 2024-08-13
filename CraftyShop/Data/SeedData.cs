using CraftyShop.Models;
using Microsoft.AspNetCore.Identity;

namespace CraftyShop.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if the admin user exists, if not, create it
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com"
                };
                await userManager.CreateAsync(adminUser, "Admin123*"); // Use a strong password

                // Assign the admin role to the user
                await userManager.AddToRoleAsync(adminUser, "admin");
            }
        }
    }
}
