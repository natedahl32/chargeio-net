using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChargeIO
{
    public class Refund : Transaction
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("charge_id")]
        public string ChargeId { get; set; }

        [JsonProperty("void_reference")]
        public string VoidReference { get; set; }
    }
}
