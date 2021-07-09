using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeCartAPI.Model
{
    public class Order
    {  
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("ItemId")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        [ForeignKey("OrderCartId")]
        public int OrderCartId { get; set; }
        public virtual OrderCart Cart { get; set; }
        public int Quantity { get; set; } = 1;
       
    }
}
