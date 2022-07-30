using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using FigurineCuisine.Models;
using Microsoft.EntityFrameworkCore;

namespace FigurineCuisine.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public int iStatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        private readonly FigurineCuisine.Data.FigurineCuisineContext _context;
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger, FigurineCuisine.Data.FigurineCuisineContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            // Get the details of the exception that occurred
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            iStatusCode = HttpContext.Response.StatusCode;
            Message = exception.Error.Message;
            StackTrace = exception.Error.StackTrace;
            if (await _context.SaveChangesAsync()>0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = Message;
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyFigurineFieldID = iStatusCode;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }
        }
    }
}
