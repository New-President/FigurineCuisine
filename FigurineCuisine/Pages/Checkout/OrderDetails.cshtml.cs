using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FigurineCuisine.Areas.Identity.Pages.Account;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FigurineCuisine.Pages.Checkout
{
    /// <summary>
    /// Inherits from PageModel class and brings in dependencies including UserManager, IEmailSender interface, and IShop interface
    /// Create a CheckoutInput class and set getter and setter
    /// </summary>
    [Authorize(Roles = "Customer")]
    public class OrderDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly Data.FigurineCuisineContext _context;
        private readonly ILogger<LoginWith2faModel> _logger;

        public string ReturnUrl { get; set; }
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
        public OrderDetailsModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, Data.FigurineCuisineContext context, ILogger<LoginWith2faModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Bind the Input object that contains all the required information for checkout to the property
        /// </summary>
        [BindProperty]
        public CheckoutInput Input { get; set; }

        public async Task<Cart> GetCartByUserIdAsync(string userId) => await _context.Cart.FirstOrDefaultAsync(cart => cart.UserID == userId);

        public ApplicationUser appUser { get; set; }
        public async Task OnGetAsync()
        {
            appUser = await _userManager.GetUserAsync(User);
            System.Diagnostics.Debug.WriteLine(appUser.Address);
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
            appUser = await _userManager.GetUserAsync(User);
            if ((ModelState.IsValid))
            {
                return Redirect("/Checkout/Receipt");
             }
            
            //var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            //}
            //var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            //var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, false);
            //if (result.Succeeded)
            //{
                
            //    _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
            //    return Redirect("/Checkout/Receipt");
            //}
            //else
            //{
            //    _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
            //    ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
            //    return Page();
            //}
            
            return Page();
        }
        
        


        public class CheckoutInput
        {
            [Display(Name = "Purchased Date:")]
            public DateTime Date { get; set; }

            [Required]
            [RegularExpression(@"^[0-9]{16}$", ErrorMessage = " Please enter a valid Card Number ")]
            [Display(Name = "Card Number")]
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
            [Display(Name = "Credit Card")]
            public CreditCard CreditCard { get; set; }
            
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }
        }

        public enum CreditCard
        {
            Visa,
            Mastercard,
        }
    }
}