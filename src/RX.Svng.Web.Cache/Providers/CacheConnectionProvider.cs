using System;
using RX.Svng.Web.Cache.Configs;
using StackExchange.Redis;

namespace RX.Svng.Web.Cache.Providers
{
    public class CacheConnectionProvider
    {
        private static readonly string Config = RedisConfiguration.RedisConnectionString;
        private static readonly int DatabaseId = GetRedisDataBaseId();

        private static int GetRedisDataBaseId()
        {
            int.TryParse(RedisConfiguration.RedisDataBaseId, out var dbId);
            return dbId;
        }

        private static readonly Lazy<ConnectionMultiplexer> Multiplexer = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Config));
        public static ConnectionMultiplexer Connection => Multiplexer.Value;

        public static IDatabase GetDatabase()
        {
            return GetDatabase(DatabaseId);
        }

        public static IDatabase GetDatabase(int dbId)
        {
            return Connection.GetDatabase(dbId);
        }
    }
}
