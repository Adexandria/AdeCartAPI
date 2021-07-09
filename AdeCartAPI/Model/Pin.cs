using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Model
{
    public class Pin
    {
        [JsonProperty("pin")]
        public string PinCode { get; set; } = "1234";
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
