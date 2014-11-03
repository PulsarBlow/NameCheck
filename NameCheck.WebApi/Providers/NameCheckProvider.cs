using SuperMassive;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NameCheck.WebApi
{
    public class NameCheckProvider
    {
        protected IMemoryCache<NameCheckModel> Cache { get; private set; }

        public NameCheckProvider(IMemoryCache<NameCheckModel> cache)
        {
            Guard.ArgumentNotNull(cache, "cache");
            Cache = cache;
        }

        public async Task<NameCheckModel> CheckNameAsync(string name, EndpointType endpointType = EndpointType.NotSet, string userIp = null)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");

            var key = NameCheckHelper.FormatKey(name);
            var result = Cache.GetItem(key);
            if (result != null) { return result; }

            result = new NameCheckModel();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.Key = key;
            result.DateUtc = DateTime.UtcNow;
            result.EndpointType = endpointType;
            result.Name = name;
            result.Query = NameCheckHelper.FormatQuery(name);
            result.UserIp = userIp;

            var twitterResult = await TwitterApiManager.IsNameAvailable(result.Query);
            result.SocialNetworks = new Dictionary<string, bool>();
            result.SocialNetworks.Add("twitter", twitterResult.Content);

            var gandiResult = GandiApiManager.CheckDomains(result.Query, new string[] { "com", "net", "org" });
            result.Domains = new Dictionary<string, bool>(GandiApiManager.CheckDomains(result.Query, new string[] { "com", "net", "org" }));

            Cache.AddItem(key, result);
            return result;
        }

        public async Task<NameCheckBatchModel> CheckBatchNameAsync(string batch, string separator, EndpointType endpointType = EndpointType.NotSet, string userIp = null)
        {
            Guard.ArgumentNotNullOrWhiteSpace(batch, "batch");
            IList<string> parsedBatch = NameCheckHelper.ParseBatch(batch, String.IsNullOrWhiteSpace(separator) ? Constants.DefaultBatchSeparator : separator);

            var result = new NameCheckBatchModel();
            if (parsedBatch == null) { return result; }
            result.NameChecks = new List<NameCheckModel>();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.DateUtc = DateTime.UtcNow;
            result.EndpointType = endpointType;
            result.UserIp = userIp;


            foreach (var item in parsedBatch)
            {
                result.NameChecks.Add(await CheckNameAsync(item, endpointType));
            }
            return result;
        }
    }
}