using Microsoft.WindowsAzure;
using SerialLabs;
using System;
using System.Runtime.Caching;

namespace NameCheck.WebApi
{
    public interface IMemoryCache<T>
    {
        void AddItem(string key, T entry);
        T GetItem(string key);
    }
    public sealed class NameCheckCache : IMemoryCache<NameCheckModel>
    {
        public const int DefaultCacheDurationMin = 5;

        private MemoryCache _memoryCache;
        private CacheItemPolicy _policy;

        public NameCheckCache()
        {
            _memoryCache = MemoryCache.Default;
            _policy = new CacheItemPolicy();

            int cacheDurationMin = 0;
            if (!Int32.TryParse(CloudConfigurationManager.GetSetting(Constants.ConfigurationKeys.CacheDurationMin), out cacheDurationMin))
            {
                cacheDurationMin = DefaultCacheDurationMin;
            }
            _policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheDurationMin);
        }

        public void AddItem(string key, NameCheckModel entry)
        {
            Guard.ArgumentNotNullOrWhiteSpace(key, "key");
            var result = _memoryCache.Add(CryptographyHelper.ComputeSHA1Hash(key), entry, _policy);
        }

        public NameCheckModel GetItem(string key)
        {
            Guard.ArgumentNotNullOrWhiteSpace(key, "key");
            return _memoryCache[CryptographyHelper.ComputeSHA1Hash(key)] as NameCheckModel;
        }
    }
}