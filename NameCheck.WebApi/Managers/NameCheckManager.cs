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

            var result = new CheckResultModel();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.Name = name;
            result.Result = new AvailabilityResult();
            result.Result.DomainCom = await WhoIsApiManager.IsNameAvailable(name);
            result.Result.Twitter = await TwitterApiManager.IsNameAvailable(name);
            result.Result.Facebook = await FacebookApiManager.IsNameAvailable(name);
            return result;
        }
    }
}