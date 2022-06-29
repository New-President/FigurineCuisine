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
                        Brand = "Aniplex",
                        Manufacturer = "China",
                        Price = 399.99M
                    },

                    new Figurine
                    {
                        Name = "Demon Slayer Nezuko",
                        Brand = "TAMASHII NATIONS",
                        Manufacturer = "China",
                        Price = 399.99M
                    },

                    new Figurine
                    {
                        Name = "Wooden Carving of Spongebob",
                        Brand = "Aniplex",
                        Manufacturer = "China",
                        Price = 399.99M
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
