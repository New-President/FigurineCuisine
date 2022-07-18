using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using FigurineCuisine.Data;
using FigurineCuisine.Models;

namespace FigurineCuisine.Pages.Figurines
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public CreateModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Figurine Figurine { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Figurine.Add(Figurine);
            //await _context.SaveChangesAsync();

            // Once a record is added, create an audit record
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Add Movie Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = Figurine.ID;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index"); //
        }
    }
}
