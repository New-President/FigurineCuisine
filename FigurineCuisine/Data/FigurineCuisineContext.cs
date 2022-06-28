using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Models;

namespace FigurineCuisine.Data
{
    public class FigurineCuisineContext : DbContext
    {
        public FigurineCuisineContext (DbContextOptions<FigurineCuisineContext> options)
            : base(options)
        {
        }

        public DbSet<FigurineCuisine.Models.Figurine> Figurine { get; set; }
    }
}
