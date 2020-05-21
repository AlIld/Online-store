using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class CartProduct
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Count of products")]
        public int Count { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}