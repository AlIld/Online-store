using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name of category")]
        public string Name { get; set; }
    }
}