using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Count of products")]
        public int Count { get; set; }

        public Product Product { get; set; }
    }
}