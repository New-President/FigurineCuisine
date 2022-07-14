using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace FigurineCuisine.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Name { get; set; }

        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage ="Please enter a valid email address")]
        public string Email { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^(\d{1,2})$", ErrorMessage = "Please enter a valid age")]
        public int Age { get; set; }

        [RegularExpression(@"^\(?([0-9]{8})$", ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNum { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Comment { get; set; }
    }
}