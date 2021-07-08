using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.DTO
{
    public class OrderCartData
    {
        public int OrderCartId { get; set; }
        public int OrderStatus { get; set; }
        public string UserId { get; set; }
    }
}
