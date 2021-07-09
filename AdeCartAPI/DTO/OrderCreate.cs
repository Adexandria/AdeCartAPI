using System.ComponentModel.DataAnnotations;

namespace AdeCartAPI.DTO
{
    public class OrderCreate
    {
        [Required(ErrorMessage ="Enter ItemId")]
        public int ItemId { get; set; }
        public int Quantity { get; set; } 

    }
}
