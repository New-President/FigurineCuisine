using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System;

namespace FigurineCuisine.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string Region { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
