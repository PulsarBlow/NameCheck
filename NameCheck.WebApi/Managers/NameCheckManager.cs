using SerialLabs.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public static class NameCheckManager
    {
        public static async Task<NameCheckModel> CheckNameAsync(string name, EndpointType endpointType = EndpointType.NotSet)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "name is null or empty");
            }

            var result = new NameCheckModel();

            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.DateUtc = DateTime.UtcNow;
            result.EndpointType = endpointType;
            result.Name = name;
            result.Query = NameHelper.Format(name);

            var twitterResult = await TwitterApiManager.IsNameAvailable(result.Query);
            result.SocialNetworks = new Dictionary<string, bool>();
            result.SocialNetworks.Add("twitter", twitterResult.IsAvailable);

            var gandiResult = GandiApiManager.CheckDomains(result.Query, new string[] { "com", "net", "org" });
            result.Domains = new Dictionary<string, bool>(GandiApiManager.CheckDomains(result.Query, new string[] { "com", "net", "org" }));
            return result;
        }
    }
}