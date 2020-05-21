using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public List<CartProduct> CartProducts { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }
}