using SuperMassive;
using System;

namespace NameCheck.WebApi
{
    public interface IRateLimit
    {
        int Limit { get; }
        int Remaining { get; }
        ulong Reset { get; }
        DateTime ResetDateTime { get; }
        double ResetDateTimeInSeconds { get; }
        string ProviderName { get; }
    }

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

        public ulong Reset { get; set; }

        public DateTime ResetDateTime { get; set; }

        public double ResetDateTimeInSeconds { get; set; }

        public string ProviderName { get; set; }
    }
}