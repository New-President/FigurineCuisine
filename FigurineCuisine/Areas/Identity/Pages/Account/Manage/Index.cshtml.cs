using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FigurineCuisine.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Data.FigurineCuisineContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, Data.FigurineCuisineContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            [RegularExpression(@"^[0-9]{8,8}$", ErrorMessage = "Enter a valid phone number")]
            public string PhoneNumber { get; set; }

            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            public string Address { get; set; }

            [DataType(DataType.PostalCode)]
            [RegularExpression(@"^(?!00000)[0-9]{6,6}$", ErrorMessage = "Invalid Zip Code")]
            public string PostalCode { get; set; }
            public string Region { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var owner = await _userManager.GetUserAsync(User);
            var userName = await _userManager.GetUserNameAsync(user);
            //await _userManager.GetPhoneNumberAsync(user)
            var phoneNumber = owner.PhoneNumber;
            var address = owner.Address;
            var region = owner.Region;
            var postalCode = owner.PostalCode;
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Address = address,
                Region = region,
                PostalCode = postalCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            // Updates user details
            var owner = await _userManager.GetUserAsync(User);
            owner.Address = Input.Address;
            owner.PhoneNumber = Input.PhoneNumber;
            owner.PostalCode = Input.PostalCode;
            owner.Region = Input.Region;
            _context.ApplicationUser.Update(owner);
            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
