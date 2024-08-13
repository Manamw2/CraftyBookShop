using CraftyShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CraftyShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole{
                    Name = "user",
                    NormalizedName = "USER"
                },
                new IdentityRole{
                    Name = "company",
                    NormalizedName = "Company"
                },
                new IdentityRole{
                    Name = "employee",
                    NormalizedName = "EMPLOYEE"
                },
            };

            // Create a password hasher
            var hasher = new PasswordHasher<AppUser>();

            // Create admin user
            var adminUser = new AppUser
            {
                Id = "1", // Specify a GUID or some unique identifier
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"), // Use a strong password
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Seed admin user
            modelBuilder.Entity<AppUser>().HasData(adminUser);

            // Assign admin role to admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = roles.First(r => r.Name == "admin").Id
            });

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Comedy", DisplayOrder = 3 },
                new Category { Id = 4, Name = "History", DisplayOrder = 4 }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price10 = 85,
                    Price20 = 80,
                    CategoryID = 1,
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price10 = 25,
                    Price20 = 20,
                    CategoryID = 4,
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price10 = 40,
                    Price20 = 35,
                    CategoryID = 3,
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price10 = 60,
                    Price20 = 55,
                    CategoryID = 2,
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price10 = 25,
                    Price20 = 20,
                    CategoryID = 1,
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price10 = 22,
                    Price20 = 20,
                    CategoryID = 3,
                }
                );
            modelBuilder.Entity<Company>().HasData(
              new Company
              {
                  Id = 1,
                  Name = "Tech Solution",
                  StreetAddress = "123 Tech St",
                  City = "Tech City",
                  PostalCode = "12121",
                  State = "IL",
                  PhoneNumber = "6669990000"
              },
              new Company
              {
                  Id = 2,
                  Name = "Vivid Books",
                  StreetAddress = "999 Vid St",
                  City = "Vid City",
                  PostalCode = "66666",
                  State = "IL",
                  PhoneNumber = "7779990000"
              },
              new Company
              {
                  Id = 3,
                  Name = "Readers Club",
                  StreetAddress = "999 Main St",
                  City = "Lala land",
                  PostalCode = "99999",
                  State = "NY",
                  PhoneNumber = "1113335555"
              }
              );


        }
    }
}
