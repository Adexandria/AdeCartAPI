using Newtonsoft.Json;

namespace AdeCartAPI.Model
{
    public class Reciept
    {
        [JsonProperty("data")]
        public RecieptData Data { get; set; }
    }
}