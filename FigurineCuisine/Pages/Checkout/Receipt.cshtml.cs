using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages.Checkout
{
    public class ReceiptModel : PageModel
    {
        public IEnumerable<CartItem> CartItem { get; set; }
        public ApplicationUser user { get; set; }
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor to take UserManager and IOrder interface
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="order"></param>
        public ReceiptModel(UserManager<ApplicationUser> userManager, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// Create a user of type ApplicationUser that gets the user that is currently signed in
        /// Get the orders for the current user by taking the user id
        /// Get all the order items by the order id
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            user = await _userManager.GetUserAsync(User);
            var cartItems = from c in _context.CartItem select c;
            cartItems = cartItems.Where(c => c.uid.Contains(user.Id));
            //CartItems = await cartItems.ToListAsync();
            CartItem = await GetCartItemsByUserIdAsync(user.Id);
            //Cart cart = await _order.GetLatestOrderForUserAsync(user.Id);
            //OrderItems = await _order.GetOrderItemsByOrderIdAsync(order.ID);
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId) => await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == userId);


        /// <summary>
        /// A property to be available on the Model property in the Razor Page
        /// </summary>
        /// public IEnumerable<CartItems> CartItem { get; set; }

        /// <summary>
        /// Asynchronous handler method to process the default GET request
        /// </summary>
        /// <returns>List of all cart items from the database</returns>

        public async Task<IActionResult> OnPostUpdateAsync(int id)
        {
            int updatedQuantity = Convert.ToInt32(Request.Form["Quantity"]);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            //CartItem cartItem = await GetCartItemsByOrderIdAsync(user.Id, id);

            //cartItem.Quantity = updatedQuantity;
            //await UpdateCartItemsAsync(cartItem);

            return RedirectToPage();


        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IEnumerable<CartItem> cartItems = await GetCartItemsByUserIdAsync(user.Id);
            foreach (var cartItem in cartItems)
            {
                await RemoveCartItemsAsync(user.Id, cartItem.FigurineID);
            }

            return Redirect("/Checkout/Cart");
        }
        //public async Task<IActionResult> OnPostDeleteAsync(int id)
        //{
        //    ApplicationUser user = await _userManager.GetUserAsync(User);
        //    await RemoveCartItemsAsync(user.Id, id);

        //    return RedirectToPage();
        //}

        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            Cart cart = await GetCartByUserIdAsync(userId);
            return _context.CartItem.Where(cartItem => cartItem.CartID == cart.ID).Include(x => x.Figurine);
        }

        public async Task<CartItem> GetCartItemByProductIdForUserAsync(string userId, int productId)
        {
            var cartItems = await GetCartItemsByUserIdAsync(userId);
            return cartItems.FirstOrDefault(cartItem => cartItem.FigurineID == productId);
        }

        public async Task UpdateCartItemsAsync(CartItem cartItem)
        {
            _context.CartItem.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemsAsync(string userId, int productId)
        {
            CartItem cartItem = await GetCartItemByProductIdForUserAsync(userId, productId);
            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemsAsync(IEnumerable<CartItem> cartItems)
        {
            _context.CartItem.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}