using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using RX.Svng.Web.Data.Models;
using RX.Svng.Web.Data.Providers;

namespace RX.Svng.Web.Data.Repositories
{
    public interface IPharmacyRepository
    {
        Task<PharmacyDataModel> FindClosestPharmacyAsync(double latitude, double longitude);
    }
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly IDbProvider _dbProvider;
        private readonly ILogger<PharmacyRepository> _logger;

        public PharmacyRepository(IDbProvider dbProvider, ILogger<PharmacyRepository> logger)
        {
            _dbProvider = dbProvider;
            _logger = logger;
        }
        public async Task<PharmacyDataModel> FindClosestPharmacyAsync(double latitude, double longitude)
        {
            _logger.LogDebug("Invoked Repository FindClosestPharmacyAsync");
            using (var conn = _dbProvider.Get())
            {
                const string query = @"
Declare @geo geography = geography::Point(@latitude, @longitude, @srid);
Select Top 1 
    Cast(pl.Location.STDistance(@geo)/1609.344 as dec(8,2)) Distance, 
    p.Name, 
    p.Address
From dbo.Pharmacy p
Inner Join dbo.PharmacyLocation pl
on p.Id = pl.PharmacyId
Where pl.Location.STDistance(@geo) is not null
Order By pl.Location.STDistance(@geo);
";
                var data = await conn.QueryFirstOrDefaultAsync<dynamic>(query, new { latitude, longitude, srid = 4326 });

                if (data == null)
                {
                    _logger.LogDebug("Repository FindClosestPharmacyAsync null successful");
                    return null;
                }

                var result = new PharmacyDataModel
                {
                    Name = data.Name,
                    Address = data.Address,
                    Distance = data.Distance
                };
                _logger.LogDebug("Repository FindClosestPharmacyAsync successful");

                return result;
            }
        }
    }
}
