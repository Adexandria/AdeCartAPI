using System.ComponentModel.DataAnnotations;


namespace AdeCartAPI.DTO
{
    public class CartUpdate
    {
        [Required]
        public int OrderCartId { get; set; }
        public string OrderStatus { get; set; }
    }
}
