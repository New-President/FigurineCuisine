using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FigurineCuisine.Data;
using FigurineCuisine.Models;

namespace FigurineCuisine.Pages.Figurines
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;

        public DeleteModel(FigurineCuisine.Data.FigurineCuisineContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Figurine = await _context.Figurine.FindAsync(id);

            if (Figurine != null)
            {
                _context.Figurine.Remove(Figurine);
                //await _context.SaveChangesAsync();

                // Once a record is deleted, create an audit record
                if (await _context.SaveChangesAsync() > 0)
                {
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "Delete Movie Record";
                    auditrecord.DateTimeStamp = DateTime.Now;
                    auditrecord.KeyFigurineFieldID = Figurine.ID;
                    var userID = User.Identity.Name.ToString();
                    auditrecord.Username = userID;
                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
