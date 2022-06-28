using System;
using System.ComponentModel.DataAnnotations;

namespace FigurineCuisine.Models
{
    public class Figurine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int RetailerID { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public DateTime PublishedDate { get; set; }
        public float Price { get; set; }
    }
}
