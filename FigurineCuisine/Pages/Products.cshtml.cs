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

namespace FigurineCuisine.Pages
{
    public class ProductsModel : PageModel
    {

        public IList<Figurine> Figurine { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public SelectList? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FigurineCategory { get; set; }
        [BindProperty]
        public CartItems CartItems { get; set; }

        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        public static Figurine? selectedProduct;

        public static void SelectProduct(Figurine product)
        {
            selectedProduct = product;
        }

        public ProductsModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        public IList<ApplicationRole> ApplicationRole { get;set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> categoryQuery = from m in _context.Figurine
                                               orderby m.Category
                                               select m.Category;
            ApplicationRole = await _context.Roles.ToListAsync();

            var figurines = from m in _context.Figurine select m; 
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CartItems.Add(CartItems);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
