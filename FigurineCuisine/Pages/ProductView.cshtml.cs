using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages
{
    public class ProductViewModel : PageModel
    {
        [BindProperty]
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
        public ProductInput Input { get; set; }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            // GetCartByUserIdAsync
            var cart = await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == user.Id);
            if (ModelState.IsValid)
            {
                CartItems cartItem = new CartItems
                {
                    CartID = cart.ID,
                    ProductID = id,
                    Quantity = Input.Quantity
                };
                if (await _shop.GetCartItemByProductIdForUserAsync(user.Id, id) != null)
                {
                    CartItems existingCartItem = await _shop.GetCartItemByProductIdForUserAsync(user.Id, id);
                    existingCartItem.Quantity += Input.Quantity;
                    await _shop.UpdateCartItemsAsync(existingCartItem);
                }
                else
                {
                    await _shop.CreateCartItemAsync(cartItem);
                }
            }
            return Redirect("/Shop/Cart");
        }
        public class ProductInput
        {
            public int Quantity { get; set; } = 1;
        }
    }
}
