using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chargeIO.Models
{
    public interface IPaymentMethodInformation
    {
        [JsonProperty("email")]
        string Email { get; set; }

        [JsonProperty("name")]
        string Name { get; set; }

        [JsonProperty("address1")]
        string Address1 { get; set; }

        [JsonProperty("city")]
        string City { get; set; }
    }
}
