using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FigurineCuisine.Models
{
    public class Figurine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RetailerID { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }

        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Ratings { get; set; }
    }
}
