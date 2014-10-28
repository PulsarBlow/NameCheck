using Microsoft.WindowsAzure;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.oAuth;

namespace NameCheck.WebApi
{
    public static class TwitterApiManager
    {
        public static async Task<ApiResponse<bool>> IsNameAvailable(string name)
        {
            TwitterCredentials.SetCredentials(GetTwitterCredentials());
            var result = await UserAsync.GetUserFromScreenName(name);
            var apiError = GetLastKnownError();
            return new ApiResponse<bool>
            {
                Content = result == null,
                Error = apiError
            };
        }

        public static async Task<ApiResponse<IRateLimit>> GetRateLimit()
        {
            TwitterCredentials.SetCredentials(GetTwitterCredentials());
            var rateLimit = await RateLimitAsync.GetCurrentCredentialsRateLimits();
            if (rateLimit == null)
            {
                return new ApiResponse<IRateLimit>(new RateLimit(Constants.ProviderNames.Twitter));
            }
            var usersLookupLimit = rateLimit.UsersLookupLimit;
            if (usersLookupLimit == null)
            {
                return new ApiResponse<IRateLimit>(new RateLimit(Constants.ProviderNames.Twitter));
            }
            RateLimit result = new RateLimit(Constants.ProviderNames.Twitter);
            result.Limit = usersLookupLimit.Limit;
            result.Remaining = usersLookupLimit.Remaining;
            result.Reset = usersLookupLimit.Reset;
            result.ResetDateTime = usersLookupLimit.ResetDateTime;
            result.ResetDateTimeInSeconds = usersLookupLimit.ResetDateTimeInSeconds;
            return new ApiResponse<IRateLimit>(result);
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