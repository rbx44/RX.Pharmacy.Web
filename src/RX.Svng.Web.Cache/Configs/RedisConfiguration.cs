using System;

namespace RX.Svng.Web.Cache.Configs
{
    public static class RedisConfiguration
    {
        private static string _redisConnectionString;
        private static string _redisDataBaseId;

        public static string RedisConnectionString
        {
            get => _redisConnectionString;
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new ApplicationException("no redis connection specified");

                _redisConnectionString = value;
            }
        }
        public static string RedisDataBaseId
        {
            get => _redisDataBaseId;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ApplicationException("couldn't find a valid redis database");

                _redisDataBaseId = value;
            }
        }
    }
}
