using Microsoft.WindowsAzure;
using System.Linq;
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
            //TwitterCredentials.ApplicationCredentials = GetTwitterCredentials();
        }


        public static async Task<ApiResponse> IsNameAvailable(string name)
        {
            TwitterCredentials.SetCredentials(GetTwitterCredentials());
            var result = await UserAsync.GetUserFromScreenName(name);
            var apiError = GetLastKnownError();
            return new ApiResponse
            {
                IsAvailable = result == null,
                Error = apiError
            };
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

        private static ApiError GetLastKnownError()
        {
            var exception = ExceptionHandler.GetLastException();
            if (exception == null) { return null; }
            return new ApiError
            {
                Code = exception.StatusCode,
                Description = exception.TwitterDescription,
                Details = exception.TwitterExceptionInfos.First().Message
            };
        }
    }
}