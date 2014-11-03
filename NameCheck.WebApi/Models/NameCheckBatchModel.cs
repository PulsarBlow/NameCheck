using Newtonsoft.Json;
using SuperMassive;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public class NameCheckBatchModel
    {
        public DescendingSortedGuid Id { get; set; }

        public IList<NameCheckModel> NameChecks { get; set; }

        public DateTime DateUtc { get; set; }

        [JsonIgnore]
        public EndpointType EndpointType { get; set; }

        [JsonIgnore]
        public string UserIp { get; set; }
    }

}