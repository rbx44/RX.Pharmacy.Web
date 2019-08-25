using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RX.Svng.Web.Data.Providers
{
    public interface IDbProvider
    {
        IDbConnection Get(string db = "db_context");
    }
    public class DbProvider : IDbProvider
    {
        private readonly IConfiguration _configuration;

        public DbProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Get(string db = "db_context")
        {
            return new SqlConnection(_configuration.GetSection("ConnectionStrings")[db] ?? throw new ApplicationException("missing required app settings"));
        }
    }
}
