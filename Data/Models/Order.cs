using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required] [Display(Name = "Time of order")]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "FullPrice")]
        public double FullPrice { get; set; }

        [Required] [Display(Name = "IsPaid")] public bool IsPaid { get; set; }

        [Required]
        [Display(Name = "IsDelivered")]
        public bool IsDelivered { get; set; }

        public User User { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}