using System;

namespace NameCheck.WebApi
{
    public class RateLimit : IRateLimit
    {
        protected RateLimit(string providerName)
        {
            if (String.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException("providerName", "providerName is null or empty");
            }
            this.ProviderName = providerName;
        }

        public int Limit { get; set; }

        public int Remaining { get; set; }

        public long Reset { get; set; }

        public DateTime ResetDateTime { get; set; }

        public double ResetDateTimeInSeconds { get; set; }

        public string ProviderName { get; protected set; }

        public static RateLimit CreateInstance(string providerName)
        {
            return new RateLimit(providerName);
        }
    }
}