using Newtonsoft.Json;
using SerialLabs.Data;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckModel
    {
        public DescendingSortedGuid Id { get; set; }

        public string Name { get; set; }

        public string Query { get; set; }

        public DateTime DateUtc { get; set; }

        public IDictionary<string, bool> SocialNetworks { get; set; }

        public IDictionary<string, bool> Domains { get; set; }

        [JsonIgnore]
        public EndpointType EndpointType { get; set; }

        [JsonIgnore]
        public string UserIp { get; set; }
    }

}