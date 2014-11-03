using SuperMassive;
using System;

namespace NameCheck.WebApi
{
    public class RateLimit : IRateLimit
    {
        public RateLimit()
        { }

        public RateLimit(string providerName)
        {
            Guard.ArgumentNotNullOrWhiteSpace(providerName, "providerName");
            ProviderName = providerName;
        }

        public int Limit { get; set; }

        public int Remaining { get; set; }

        public long Reset { get; set; }

        public DateTime ResetDateTime { get; set; }

        public double ResetDateTimeInSeconds { get; set; }

        public string ProviderName { get; protected set; }
    }
}