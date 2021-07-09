using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Model
{
    public class Charge
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("card")]
        public Card CardDetails { get; set; }
        [JsonProperty("pin")]
        public string PinCode { get; set; } = "0000";
    }
}
