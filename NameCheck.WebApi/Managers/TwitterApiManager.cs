using LinqToTwitter;
using Microsoft.WindowsAzure;
using SuperMassive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.oAuth;

namespace NameCheck.WebApi
{
    public static class TwitterApiManager
    {
        public static async Task<TwitterApiResponse<bool>> IsNameAvailable(string screenName)
        {
            TwitterApiResponse<bool> response = new TwitterApiResponse<bool>();
            response.Content = true;

            try
            {
                var ctx = new TwitterContext(new SingleUserAuthorizer
                {
                    CredentialStore = GetCredentials()
                });
                var query = await (
                    from search in ctx.User
                    where search.Type == UserType.Show && search.ScreenName == screenName
                    select search).SingleOrDefaultAsync();

                if (query != null)
                {
                    response.Content = false;
                    var name = query.ScreenNameResponse;
                    var lastStatus = query.Status == null ? "No Status" : query.Status.Text;
                }
            }
            catch (TwitterQueryException ex)
            {
                response.Content = ex.StatusCode == System.Net.HttpStatusCode.NotFound ? true : false;
                response.Error = CreateError(ex);
            }

            return response;
        }


        public static async Task<TwitterApiResponse<Dictionary<string,RateLimit>>> GetRateLimits(string resources = "")
        {
            Dictionary<string, RateLimit> rateLimits = new Dictionary<string, RateLimit>();
            TwitterApiError error = null;
            try
            {
                var ctx = GetContext();
                var query = await (
                    from help in ctx.Help
                    where help.Type == HelpType.RateLimits && help.Resources == resources
                    select help).SingleOrDefaultAsync();

                if (query != null && query.RateLimits != null)
                {
                    foreach (var category in query.RateLimits)
                    {
                        RateLimit rateLimit = new RateLimit();
                        foreach (var limit in category.Value)
                        {
                            rateLimit.ProviderName = Constants.ProviderNames.Twitter;
                            rateLimit.Limit = limit.Limit;
                            rateLimit.Remaining = limit.Remaining;
                            rateLimit.Reset = limit.Reset;
                            rateLimit.ResetDateTime = DateHelper.FromUnixTime(limit.Reset).AsUtc();
                            rateLimit.ResetDateTimeInSeconds = DateHelper.FromUnixTime(limit.Reset).AsUtc().Subtract(DateTime.UtcNow).TotalSeconds;
                        }
                        rateLimits.Add(category.Key, rateLimit);
                    }
                }
            }
            catch(TwitterQueryException ex)
            {
                error = CreateError(ex);
            }

            return new TwitterApiResponse<Dictionary<string, RateLimit>>
            {
                Content = rateLimits,
                Error = error
            };
        }

        private static TwitterContext GetContext()
        {
            return new TwitterContext(new SingleUserAuthorizer
            {
                CredentialStore = GetCredentials()
            });
        }

        private static SingleUserInMemoryCredentialStore GetCredentials()
        {
            return new SingleUserInMemoryCredentialStore
            {
                ConsumerKey = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerKey),
                ConsumerSecret = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterConsumerSecret),
                AccessToken = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessToken),
                AccessTokenSecret = CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.TwitterAccessTokenSecret)
            };
        }

        private static TwitterApiError CreateError(TwitterQueryException ex)
        {
            if (ex == null) { return null; }
            return new TwitterApiError
            {
                Code = ex.ErrorCode,
                Description = ex.Message,
                Details = ex.ReasonPhrase
            };
        }
    }
}