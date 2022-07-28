using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages
{
    public class CartModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartModel(UserManager<ApplicationUser> userManager, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IEnumerable<CartItem> CartItems { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Cart cart = await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == user.Id);
            //CartItems = _context.CartItem.Where(cartItem => cartItem.CartID == cart.ID).Include(x => x.FigurineID);
        }
    }
}
