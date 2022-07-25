using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Data;
using FigurineCuisine.Models;

namespace FigurineCuisine.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public ProductsModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        public IList<ApplicationRole> ApplicationRole { get;set; }

        public async Task OnGetAsync()
        {
            ApplicationRole = await _context.Roles.ToListAsync();
        }
    }
}
