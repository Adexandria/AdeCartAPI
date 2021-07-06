using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.DTO
{
    public class ItemDTO
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int AvailableItem { get; set; }
    }
}
