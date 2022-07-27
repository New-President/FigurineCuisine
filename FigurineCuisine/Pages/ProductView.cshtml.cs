using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FigurineCuisine.Pages
{
    public class ProductViewModel : PageModel
    {
        public Figurine selectedProduct { get; set; }
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductViewModel(UserManager<ApplicationUser> userManager, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task OnGetAsync(int id)
        {
            selectedProduct = await _context.Figurine.FindAsync(id);
        }
    }
}
