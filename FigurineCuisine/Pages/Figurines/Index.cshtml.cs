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
    public class IndexModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public IndexModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        public IList<Figurine> Figurine { get;set; }

        public async Task OnGetAsync()
        {
            Figurine = await _context.Figurine.ToListAsync();
            var movies = from m in _context.Figurine
                         select m; 
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(s => s.Name.Contains(SearchString));
            }

            Figurine = await movies.ToListAsync();

        }
    }
}
