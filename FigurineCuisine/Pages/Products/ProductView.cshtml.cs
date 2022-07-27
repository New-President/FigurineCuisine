using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;

namespace FigurineCuisine.Pages
{
    public class ProductViewModel : PageModel
    {
        [BindProperty]
        public Figurine selectedProduct { get; set; }
        [BindProperty]
        public CartItems CartItems { get; set; }
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.CartItems.Add(CartItems);
            System.Diagnostics.Debug.WriteLine(selectedProduct.Name);
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Add CartItem Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = CartItems.ID;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("../../Products/Products");
        }
    }
}
