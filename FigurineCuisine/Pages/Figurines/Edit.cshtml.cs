using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Data;
using FigurineCuisine.Models;

namespace FigurineCuisine.Pages.Figurines
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public EditModel(FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Figurine Figurine { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Figurine = await _context.Figurine.FirstOrDefaultAsync(m => m.ID == id);

            if (Figurine == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Figurine).State = EntityState.Modified;
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Edit Figurine Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = Figurine.ID;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FigurineExists(Figurine.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FigurineExists(int id)
        {
            return _context.Figurine.Any(e => e.ID == id);
        }
    }
}
