using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace NameCheck.WebApi
{
    public class NameCheckBatchEntity : TableEntity
    {
        public DateTime DateUtc { get; set; }
        public string UserIp { get; set; }
        public string NameChecksJson { get; set; }
        public string EndpointType { get; set; }
        public string Value { get; set; }

    }
}