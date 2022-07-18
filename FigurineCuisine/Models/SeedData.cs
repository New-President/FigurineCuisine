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
                        Category = "Anime"
                    },

                    new Figurine
                    {
                        Name = "Demon Slayer Nezuko",
                        RetailerID = 2,
                        Brand = "TAMASHII NATIONS",
                        Manufacturer = "Japan",
                        Price = 200.99M,
                        Category = "Anime"
                    },

                    new Figurine
                    {
                        Name = "Wooden Carving of Spongebob",
                        RetailerID = 3,
                        Brand = "Aniplex",
                        Manufacturer = "China",
                        Price = 40.99M,
                        Category = "Wooden"
                    },
                    new Figurine
                    {
                        Name = "Statue of Greek God",
                        RetailerID = 4,
                        Brand = "Chirstian",
                        Manufacturer = "Europe",
                        Price = 999.99M,
                        Category = "Others"
                    },
                    new Figurine
                    {
                        Name = "Wooden Barbie Doll",
                        RetailerID = 5,
                        Brand = "Disney",
                        Manufacturer = "China",
                        Price = 1.99M,
                        Category = "Wooden"
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
