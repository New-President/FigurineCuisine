using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages.Checkout
{
    [Authorize(Roles = "Customer")]
    public class CartModel : PageModel
    {
        /// <summary>
        /// Dependency injection to establish a private connection to a database table by injecting an interface
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly Data.FigurineCuisineContext _context;
        public IList<CartItem> CartItems { get; set; }

        public IList<Figurine> Figurines { get; set; }
        //private readonly IShop _shop;

        /// <summary>
        /// A contructor to set propety to the corresponding interface instance
        /// </summary>
        /// <param name="context">IInventory interface</param>
        public CartModel(UserManager<ApplicationUser> userManager/*, IShop shopcontext*/, IEmailSender emailSender, Data.FigurineCuisineContext context)
        {
            //_shop = shopcontext;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        public IEnumerable<CartItem> CartItem { get; set; }

        public async Task<Cart> GetCartByUserIdAsync(string userId) => await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == userId);


        /// <summary>
        /// A property to be available on the Model property in the Razor Page
        /// </summary>
        /// public IEnumerable<CartItems> CartItem { get; set; }

        /// <summary>
        /// Asynchronous handler method to process the default GET request
        /// </summary>
        /// <returns>List of all cart items from the database</returns>
        public async Task OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            CartItem = await GetCartItemsByUserIdAsync(user.Id);
            
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id)
        {
            int updatedQuantity = Convert.ToInt32(Request.Form["Quantity"]);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            CartItem cartItem = await GetCartItemByProductIdForUserAsync(user.Id, id);

            cartItem.Quantity = updatedQuantity;
            await UpdateCartItemsAsync(cartItem);
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Update CartItem Quanity Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = id;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();


        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            await RemoveCartItemsAsync(user.Id, id);
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Delete CartItem Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = id;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

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