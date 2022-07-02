using Microsoft.AspNetCore.Identity;
using System;

namespace FigurineCuisine.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
    }
}
