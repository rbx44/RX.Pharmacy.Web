using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RX.Svng.Web.Cache.Providers;
using StackExchange.Redis;

namespace RX.Svng.Web.Cache.Repositories
{
    public interface ICacheRepository
    {
        Task<T> GetByKeyAsync<T>(string index, string key);
        Task SetKeyExpirationAsync(string key, TimeSpan expiry);
        Task UpsertKeyAsync<T>(string key, T entry) where T : new();
    }

    public class CacheRepository : ICacheRepository
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        public async Task<T> GetByKeyAsync<T>(string index, string key)
        {
            var db = CacheConnectionProvider.GetDatabase();
            var val = await db.HashGetAsync(key, index);
            return val.HasValue ? JsonConvert.DeserializeObject<T>(val) : default(T);
        }

        public async Task SetKeyExpirationAsync(string key, TimeSpan expiry)
        {
            var db = CacheConnectionProvider.GetDatabase();
            await db.KeyExpireAsync(key, expiry);
        }

        public async Task UpsertKeyAsync<T>(string key, T entry) where T : new()
        {
            var db = CacheConnectionProvider.GetDatabase();
            var payload = JsonConvert.SerializeObject(entry, _settings);
            await db.HashSetAsync(key, new[] { new HashEntry("payload", payload) });
        }
    }
}
