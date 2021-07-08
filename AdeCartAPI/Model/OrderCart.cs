using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdeCartAPI.Model
{
    public class OrderCart
    {
        [Key]
        public int OrderCartId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.NotPaid;
        [ForeignKey("Id")]
        public string UserId { get; set; }
    }
}
