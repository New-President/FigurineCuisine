using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FigurineCuisine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace RazorPagesMovie.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        public EditModel(RoleManager<ApplicationRole> roleManager, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationRole = await _roleManager.FindByIdAsync(id);
            if (ApplicationRole == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ApplicationRole appRole = await _roleManager.FindByIdAsync(ApplicationRole.Id);
            appRole.Id = ApplicationRole.Id;
            appRole.Name = ApplicationRole.Name;
            appRole.Description = ApplicationRole.Description;
            IdentityResult roleRuslt = await _roleManager.UpdateAsync(appRole);
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Edit CartItem Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = Int32.Parse(ApplicationRole.Id);
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
            if (roleRuslt.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}