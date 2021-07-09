using Newtonsoft.Json;

namespace AdeCartAPI.Model
{
    public class Card
    {

        [JsonProperty("cvv")]
        public string CVV { get; set; } = "408";

        [JsonProperty("number")]
        public string Number { get; set; } = "4084084084084081";

        [JsonProperty("expiry_month")]
        public string ExpiryMonth { get; set; } = "01";

        [JsonProperty("expiry_year")]
        public string ExpiryYear { get; set; } = "99";
    }
}