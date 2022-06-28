using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Data;
using FigurineCuisine.Models;

namespace FigurineCuisine.Pages.Figurines
{
    public class DetailsModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public DetailsModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        public Figurine Figurine { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Figurine = await _context.Figurine.FirstOrDefaultAsync(m => m.ID == id);

            if (Figurine == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
