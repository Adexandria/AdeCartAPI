using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.DTO
{
    public class ItemCreate
    {
        [Required(ErrorMessage = "Enter ItemName"),StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter ItemPrice")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Enter ItemDescription"), StringLength(300)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter AvailableItem")]
        public int AvailableItem { get; set; }
    }
}
