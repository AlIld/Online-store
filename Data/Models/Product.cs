﻿using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name of product")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required] [Display(Name = "Price")] public double Price { get; set; }

        [Required]
        [Display(Name = "Image source link")]
        public string ImageSrc { get; set; }

        public Category Category { get; set; }
    }
}