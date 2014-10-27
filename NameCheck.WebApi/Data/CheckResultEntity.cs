using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace NameCheck.WebApi
{
    public class CheckResultEntity : TableEntity
    {
        public string Name { get; set; }
        public DateTime DateUtc { get; set; }
        public string UserIp { get; set; }
        public bool Twitter { get; set; }
        public string Extensions { get; set; }

    }
}