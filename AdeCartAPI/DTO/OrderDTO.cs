using AdeCartAPI.Model;


namespace AdeCartAPI.DTO
{
    public class OrderDTO
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
