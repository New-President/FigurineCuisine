using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using FigurineCuisine.Data;

namespace FigurineCuisine.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FigurineCuisineContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FigurineCuisineContext>>()))
            {
                if (context == null || context.Figurine == null)
                {
                    throw new ArgumentNullException("Null FigurineCuisineContext");
                }

                // Look for any figurines.
                if (context.Figurine.Any())
                {
                    return;   // DB has been seeded
                }
                
                context.Figurine.AddRange(
                    new Figurine
                    {
                        Name = "Re:Zero Rem Figurine",
                        RetailerID = 1,
                        Brand = "Aniplex",
                        Manufacturer = "Japan",
                        Price = 399.99M,
                        Category = "Anime",
                        Image = "https://images.goodsmile.info/cgm/images/product/20170210/6256/43794/large/cc3d0d1e91f916a2b68dc0ed5ece2da8.jpg",
                    },

                    new Figurine
                    {
                        Name = "Demon Slayer Nezuko",
                        RetailerID = 2,
                        Brand = "TAMASHII NATIONS",
                        Manufacturer = "Japan",
                        Price = 200.99M,
                        Category = "Anime",
                        Image = "https://cdn.shopify.com/s/files/1/0367/9101/products/figurine-demon-slayer-kimetsu-no-yaiba-nezuko-kamado-dx-edition-figma-no-508-dx-br-pre-order-28776895250511_1024x1024.jpg?v=1634451489"
                    },

                    new Figurine
                    {
                        Name = "Wooden Carving of Spongebob",
                        RetailerID = 3,
                        Brand = "Aniplex",
                        Manufacturer = "China",
                        Price = 40.99M,
                        Category = "Wooden",
                        Image = "https://i.pinimg.com/originals/30/6d/b9/306db91b03a411bd4d03c684619e63b7.jpg"
                    },
                    new Figurine
                    {
                        Name = "Statue of Greek God",
                        RetailerID = 4,
                        Brand = "Chirstian",
                        Manufacturer = "Europe",
                        Price = 999.99M,
                        Category = "Others",
                        Image = "https://t3.ftcdn.net/jpg/01/16/77/20/360_F_116772018_nf18bS2FM3WrhOsZUC6ru2qQNS1uxwza.jpg"
                    },
                    new Figurine
                    {
                        Name = "Wooden Barbie Doll",
                        RetailerID = 5,
                        Brand = "Disney",
                        Manufacturer = "China",
                        Price = 1.99M,
                        Category = "Wooden",
                        Image = "https://upload.wikimedia.org/wikipedia/commons/0/01/Gliederpuppe.jpg"
                    }
                );
                context.Roles.AddRange(
                    new ApplicationRole
                    {
                        Id = "0",
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        Description = "Admin Roles",
                        CreatedDate = DateTime.Now,
                        IPAddress = "::1"
                    },
                    new ApplicationRole
                    {
                        Id = "1",
                        Name = "Customer",
                        NormalizedName = "CUSTOMER",
                        Description = "Customer Roles",
                        CreatedDate= DateTime.Now,
                        IPAddress = "::1"
                    },
                    new ApplicationRole
                    {
                        Id = "2",
                        Name = "Salesperson",
                        NormalizedName = "SALESPERSON",
                        Description = "Salesperson Roles",
                        CreatedDate = DateTime.Now,
                        IPAddress = "::1"
                    }
                );
                context.Users.AddRange(
                    new ApplicationUser
                    {
                        Id = "0",
                        UserName = "Admin",
                        NormalizedEmail = "A@GMAIL.COM",
                        NormalizedUserName = "ADMIN",
                        Email = "a@gamil.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Address = "Happy Land",
                        Region = "North",
                        PostalCode = "123456",
                        PhoneNumber = "88076787",
                        PasswordHash = "AQAAAAEAACcQAAAAECCeItX7x3HnvGaLgKF7N9bRspQHACbomXWu2Et4Zf5yupoHPGWoSY38CAWuKE2r7w==",
                        LockoutEnabled = true
                    }
                );
                context.UserRoles.AddRange(
                    new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
                    {
                        RoleId = "0",
                        UserId = "0",
                    }
                );

                context.Cart.AddRange(
                    new Cart
                    {
                        UserID = "0"
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
