using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace NameCheck.WebApi
{
    public abstract class BaseModel<TId>
    {
        [XmlIgnore]
        [JsonIgnore]
        public TId Id { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateUtc { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public EndpointType EndpointType { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string UserIp { get; set; }
    }
}