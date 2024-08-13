using CraftyShop.Data;
using Microsoft.EntityFrameworkCore;

namespace CraftyShop.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigraions(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
