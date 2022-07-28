using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages.Checkout
{
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

            var cartitems = from c in _context.CartItem select c;

            CartItems = await cartitems.ToListAsync();


        }

        public async Task<IActionResult> OnPostUpdateAsync(int id)
        {
            int updatedQuantity = Convert.ToInt32(Request.Form["Quantity"]);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            //await _shop.RemoveCartItemsAsync(user.Id, id);

            return RedirectToPage();
        }
    }
}