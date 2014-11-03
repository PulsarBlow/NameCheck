using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace NameCheck.WebApi
{
    public class NameCheckEntity : TableEntity
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Query { get; set; }
        public DateTime DateUtc { get; set; }
        public string UserIp { get; set; }
        public string SocialNetworksJson { get; set; }
        public string DomainsJson { get; set; }
        public string EndpointType { get; set; }

    }
}