using SerialLabs.Data;
using System;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public static class NameCheckManager
    {
        public static async Task<CheckResultModel> CheckNameAsync(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "name is null or empty");
            }

            var whoisResult = await WhoIsApiManager.IsNameAvailable(name);
            var twitterResult = await TwitterApiManager.IsNameAvailable(name);

            var result = new CheckResultModel();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.Name = name;
            result.Result = new AvailabilityResult();
            result.Result.DomainCom = whoisResult.IsAvailable;
            result.Result.Twitter = twitterResult.IsAvailable;
            result.Result.Facebook = await FacebookApiManager.IsNameAvailable(name);
            return result;
        }
    }
}