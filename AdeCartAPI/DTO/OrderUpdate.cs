using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.DTO
{
    public class OrderUpdate
    {
        [Required(ErrorMessage = "Enter ItemId")]
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
