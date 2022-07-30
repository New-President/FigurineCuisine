﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FigurineCuisine.Models
{
    public class Figurine
    {
        public int ID { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Name { get; set; }

        public int RetailerID { get; set; }

        [JsonPropertyName("img")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Display Image Url")]
        public string Image { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Brand { get; set; }

        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter valid string.")]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Ratings { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("Anime|Wooden|Others", ErrorMessage = "Invalid Status")]
        public string Category { get; set; }

        public static implicit operator Figurine(List<Figurine> v)
        {
            throw new NotImplementedException();
        }
    }
}
