using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace FlowersPlanner.Data.Dto
{
    public class ApiTokenDto
    {
        
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = ".issued")]
        public DateTime Issued { get; set; }

        [JsonProperty(PropertyName = ".expires")]
        public DateTime Expires { get; set; }
    }
}
