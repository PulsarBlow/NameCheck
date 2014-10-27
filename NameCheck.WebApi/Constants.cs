
namespace NameCheck.WebApi
{
    public static class Constants
    {
        public static class ProviderNames
        {
            public const string Twitter = "Twitter";
            public const string Facebook = "Facebook";
        }

        public static class ConfigurationKeys
        {
            public const string TwitterConsumerKey = "TwitterConsumerKey";
            public const string TwitterConsumerSecret = "TwitterConsumerSecret";
            public const string TwitterAccessToken = "TwitterAccessToken";
            public const string TwitterAccessTokenSecret = "TwitterAccessTokenSecret";
            public const string FacebookAppId = "FacebookAppId";
            public const string FacebookAppSecret = "FacebookAppSecret";
            public const string MonitoringSecret = "MonitoringSecret";
            public const string StorageConnectionString = "StorageConnectionString";
        }
    }
}