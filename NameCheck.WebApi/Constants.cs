﻿
namespace NameCheck.WebApi
{
    public static class Constants
    {
        public const string DefaultBatchSeparator = ";";

        public static class ProviderNames
        {
            public const string Twitter = "Twitter";
            public const string Facebook = "Facebook";
        }

        public static class ConfigurationKeys
        {
            public const string GoogleAppId = "GoogleAppId";
            public const string GoogleAppSecret = "GoogleAppSecret";
            public const string TwitterConsumerKey = "TwitterConsumerKey";
            public const string TwitterConsumerSecret = "TwitterConsumerSecret";
            public const string TwitterAccessToken = "TwitterAccessToken";
            public const string TwitterAccessTokenSecret = "TwitterAccessTokenSecret";
            public const string AuthorizedEmails = "AuthorizedEmails";
            public const string AuthorizedApiToken = "AuthorizedApiToken";
            public const string StorageConnectionString = "StorageConnectionString";
            public const string GandiApiKey = "GandiApiKey";
            public const string GandiApiEndpoint = "GandiApiEndpoint";
            public const string CorsAllowOrigins = "CorsAllowOrigins";
            public const string CorsAllowMethods = "CorsAllowMethods";
            public const string CorsAllowHeaders = "CorsAllowHeaders";
            public const string CacheDurationMin = "CacheDurationMin";
        }

        public static class SessionKeys
        {
            public const string NameCheckHistory = "NameCheckHistory";
            public const string NameCheckBatchHistory = "NameCheckBatchHistory";
        }
        public static class Cors
        {
            public const string AllowAll = "*";
        }

        public static class Claims
        {
            public static class Roles
            {
                public const string AuthenticatedUser = "AuthenticatedUser";
            }
        }
    }
}