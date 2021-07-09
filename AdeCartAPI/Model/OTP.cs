using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeCartAPI.Model
{
    public class OTP
    {
        [JsonProperty("otp")]
        public string OTPCode { get; set; } = "123456";
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
