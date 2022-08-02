using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Data;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FigurineCuisine.Pages.Figurines
{
        [Authorize(Roles = "Admin, Salesperson")]
        public class IndexModel : PageModel
    {

        public IList<Figurine> Figurine { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ? SearchString { get; set; }
        public SelectList ? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FigurineCategory { get; set; }


        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public IndexModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }


        public async Task OnGetAsync()
        {
            IQueryable<string> categoryQuery = from m in _context.Figurine
                                            orderby m.Category
                                            select m.Category;

            var figurines = from m in _context.Figurine
                         select m; 
            if (!string.IsNullOrEmpty(SearchString))
            {
                figurines = figurines.Where(s => s.Name.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(FigurineCategory))
            {
                figurines = figurines.Where(x => x.Category == FigurineCategory);
            }
            Category = new SelectList(await categoryQuery.Distinct().ToListAsync());
            Figurine = await figurines.ToListAsync();

        }
    }
}
