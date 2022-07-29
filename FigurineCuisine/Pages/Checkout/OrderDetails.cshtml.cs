    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages.Checkout
{
    /// <summary>
    /// Inherits from PageModel class and brings in dependencies including UserManager, IEmailSender interface, and IShop interface
    /// Create a CheckoutInput class and set getter and setter
    /// </summary>
    public class OrderDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly Data.FigurineCuisineContext _context;


        //private readonly IShop _shop;
        //private readonly IPayment _paymnet;
        //private readonly IOrder _order;

        /// <summary>
        /// Constructor to take UserManager, IEmailSender, IShop, IPayment, and IOrder interfaces to enable the checkout process
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="shop"></param>
        /// <param name="payment"></param>
        /// <param name="order"></param>
        public OrderDetailsModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, Data.FigurineCuisineContext context)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        /// Bind the Input object that contains all the required information for checkout to the property
        /// </summary>
        [BindProperty]
        public CheckoutInput Input { get; set; }

        public async Task<Cart> GetCartByUserIdAsync(string userId) => await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == userId);


        public void OnGet()
        {
        }

        /// <summary>
        /// This post operation uses UserManager to get the current signed in user
        /// Set a variable to store the total costs of all items in the cart
        /// Set variables to store email contents for an order summary email that is to be sent out to a user after they check out
        /// After the email is sent out, redirect the user to receipt page
        /// </summary>
        /// <returns>If the ckeckout process is successful, redirect to the receipt page. Otherwise, returns to the same page</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                //IEnumerable<CartItem> cartItems = await GetCartItemsByUserIdAsync(user.Id);

                //await RemoveCartItemsAsync(cartItems);

                return Redirect("/Checkout/Receipt");
             }
   
            return Page();
        }
        
        


        public class CheckoutInput
        {
            [Display(Name = "Purchased Date:")]
            public DateTime Date { get; set; }

            [Required]
            [RegularExpression(@"^[0-9]{16}$", ErrorMessage = " Please enter a valid Card Number ")]
            public string CardNumber { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public string ExpiryDate { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [RegularExpression("^[0-9]{3}$", ErrorMessage = " Please enter a valid security code.")] 
            public string SecurityCode { get; set; }

            [Required]
            public CreditCard CreditCard { get; set; }
        }

        public enum CreditCard
        {
            Visa,
            Mastercard,
        }
    }
}