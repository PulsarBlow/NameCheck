using Microsoft.WindowsAzure;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.oAuth;

namespace NameCheck.WebApi
{
    public static class TwitterApiManager
    {
        static TwitterApiManager()
        {
            // Set credentials globaly
            TwitterCredentials.ApplicationCredentials = GetTwitterCredentials();
        }


        public static async Task<bool> IsNameAvailable(string name)
        {
            var result = await UserAsync.GetUserFromScreenName(name);
            return result == null;
        }

        public static async Task<IRateLimit> GetRateLimit()
        {
            var rateLimit = await RateLimitAsync.GetCurrentCredentialsRateLimits();
            if (rateLimit == null)
            {
                return RateLimit.CreateInstance(Constants.ProviderNames.Twitter);
            }
            var usersLookupLimit = rateLimit.UsersLookupLimit;
            if (usersLookupLimit == null)
            {
                return RateLimit.CreateInstance(Constants.ProviderNames.Twitter);
            }
            RateLimit result = RateLimit.CreateInstance(Constants.ProviderNames.Twitter);
            result.Limit = usersLookupLimit.Limit;
            result.Remaining = usersLookupLimit.Remaining;
            result.Reset = usersLookupLimit.Reset;
            result.ResetDateTime = usersLookupLimit.ResetDateTime;
            result.ResetDateTimeInSeconds = usersLookupLimit.ResetDateTimeInSeconds;
            return result;
        }

        public static bool CanCheckName()
        {
            var rateLimit = GetRateLimit();
            return rateLimit.Result != null && rateLimit.Result.Remaining > 0;
        }

        private static IOAuthCredentials GetTwitterCredentials()
        {
            return Tweetinvi.TwitterCredentials.CreateCredentials(
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessToken),
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessTokenSecret),
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerKey),
                CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerSecret));
        }
    }
}