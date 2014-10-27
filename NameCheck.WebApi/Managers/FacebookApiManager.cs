using System;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public static class FacebookApiManager
    {
        public static async Task<bool> IsNameAvailable(string name)
        {
            return await Task.Factory.StartNew(() =>
            {
                return true;
            });
        }

        public static async Task<IRateLimit> GetRateLimit()
        {
            return await Task.Factory.StartNew(() =>
            {
                var rateLimit = RateLimit.CreateInstance(Constants.ProviderNames.Facebook);
                rateLimit.Limit = 200;
                return rateLimit;
            });
        }
    }
}