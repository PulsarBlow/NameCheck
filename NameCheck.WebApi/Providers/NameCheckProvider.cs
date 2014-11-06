using SuperMassive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<NameCheckModel> NameCheckAsync(string name, EndpointType endpointType = EndpointType.NotSet, string userIp = null)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            var key = NameCheckHelper.FormatKey(name);
            var result = Cache.GetItem(key);
            if (result != null) { return result; }

            result = new NameCheckModel();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.Key = key;
            result.DateUtc = DateTime.UtcNow;
            result.EndpointType = endpointType;
            result.Name = NameCheckHelper.FormatName(name);
            result.Query = NameCheckHelper.FormatQuery(name);
            result.UserIp = userIp;

            var twitterResult = await TwitterApiManager.IsNameAvailable(result.Query);
            var gandiResult = GandiApiManager.CheckDomains(result.Query, new string[] { "com", "net", "org" });

            result.SocialNetworks = new Dictionary<string, bool>();
            result.SocialNetworks.Add("twitter", twitterResult.Content);            
            result.Domains = gandiResult;

            timer.Stop();
            result.QueryDurationMs = timer.ElapsedMilliseconds;
            Cache.AddItem(key, result);
            return result;
        }

        public async Task<NameCheckBatchModel> NameCheckBatchAsync(string value, string separator, EndpointType endpointType = EndpointType.NotSet, string userIp = null)
        {
            Guard.ArgumentNotNullOrWhiteSpace(value, "value");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            IList<string> parsedBatch = NameCheckHelper.ParseBatch(value, String.IsNullOrWhiteSpace(separator) ? Constants.DefaultBatchSeparator : separator);

            var result = new NameCheckBatchModel();
            if (parsedBatch == null) { return result; }
            result.Value = value;
            result.NameChecks = new List<NameCheckModel>();
            result.Id = DescendingSortedGuid.NewSortedGuid();
            result.DateUtc = DateTime.UtcNow;
            result.EndpointType = endpointType;
            result.UserIp = userIp;


            foreach (var item in parsedBatch)
            {
                result.NameChecks.AddIfNotNull(await NameCheckAsync(item, endpointType));
            }
            timer.Stop();
            result.BatchDurationMs = timer.ElapsedMilliseconds;
            return result;
        }
    }
}