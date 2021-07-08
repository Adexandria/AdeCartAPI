using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.DTO
{
    public class CartUpdate
    {
        [Required]
        public int OrderCartId { get; set; }
        public string OrderStatus { get; set; }
        public string UserId { get; set; }
    }
}
