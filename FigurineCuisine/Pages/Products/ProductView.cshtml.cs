using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FigurineCuisine.Pages
{
    public class ProductViewModel : PageModel
    {
        public string ReturnUrl { get; set; }
        public string? Category { get; set; }

        [BindProperty]
        public Figurine selectedProduct { get; set; }
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductViewModel(UserManager<ApplicationUser> userManager, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            public int Quantity { get; set; }
            [Required]
            public int FigurineID { get; set; }
        }
        public async Task OnGetAsync(int id)
        {
            selectedProduct = await _context.Figurine.FindAsync(id);
            ReturnUrl = "Products/ProductView/" + id;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // gets user
            var user = await _userManager.GetUserAsync(User);
            Input.FigurineID = selectedProduct.ID;
            Category = selectedProduct.Category;
            if (ModelState.IsValid)
            {
                if (Input.Quantity == 0)
                {
                    return LocalRedirect(ReturnUrl);
                }
                var CartItem = new CartItem
                {
                    CartID = user.Id,
                    Quantity = Input.Quantity,
                    FigurineID = Input.FigurineID
                };
                System.Diagnostics.Debug.WriteLine("CartID: " + CartItem.CartID);
                System.Diagnostics.Debug.WriteLine("Quantity: " + CartItem.Quantity);
                _context.CartItem.Add(CartItem);
                if (await _context.SaveChangesAsync() > 0)
                {
                    // Create an auditrecord object
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "Add CartItem Record";
                    auditrecord.DateTimeStamp = DateTime.Now;
                    auditrecord.KeyFigurineFieldID = CartItem.ID;
                    // Get current logged-in user
                    var userID = User.Identity.Name.ToString();
                    auditrecord.Username = userID;
                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("/Checkout/Cart");
            }
           
            return LocalRedirect(ReturnUrl); 
        }
    }
}
