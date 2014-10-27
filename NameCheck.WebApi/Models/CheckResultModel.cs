
using SerialLabs.Data;
using System;
using System.Collections.Generic;
namespace NameCheck.WebApi
{
    public class CheckResultModel
    {
        public DescendingSortedGuid Id { get; set; }
        public DateTime DateUtc { get; set; }
        public string Name { get; set; }
        public AvailabilityResult Result { get; set; }
        public bool Twitter { get; set; }
        public IDictionary<string, bool> DomainExtensions { get; set; }
    }
}