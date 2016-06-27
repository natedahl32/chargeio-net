using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;

namespace ChargeIO
{
    public class Credit : Transaction
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("void_reference")]
        public string VoidReference { get; set; }
    }
}
