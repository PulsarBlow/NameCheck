using System;

namespace NameCheck.WebApi
{
    public interface IRateLimit
    {
        int Limit { get; }
        int Remaining { get; }
        long Reset { get; }
        DateTime ResetDateTime { get; }
        double ResetDateTimeInSeconds { get; }
        string ProviderName { get; }
    }
}