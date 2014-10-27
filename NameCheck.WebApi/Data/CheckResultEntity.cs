using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace NameCheck.WebApi
{
    public class CheckResultEntity : TableEntity
    {
        public string Name { get; set; }
        public DateTime DateUtc { get; set; }
        public string UserIp { get; set; }
        public bool IsDomainComAvailable { get; set; }
        public bool IsTwitterAvailable { get; set; }
        public bool IsFacebookAvailable { get; set; }

    }
}