
using SerialLabs.Data;
using System;
namespace NameCheck.WebApi
{
    public class CheckResultModel
    {
        public DescendingSortedGuid Id { get; set; }
        public DateTime DateUtc { get; set; }
        public string Name { get; set; }
        public AvailabilityResult Result { get; set; }
    }
}