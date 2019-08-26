using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Data.Models;
using RX.Svng.Web.Data.Providers;
using RX.Svng.Web.Data.Repositories;
using Xunit;

namespace RX.Svng.Web.Data.Test.Repositories
{
    public class PharmacyRepositoryTest
    {
        private readonly Mock<ILogger<PharmacyRepository>> _loggerMock;
        private readonly IDbProvider _dbProvider;

        public PharmacyRepositoryTest()
        {
            _loggerMock = new Mock<ILogger<PharmacyRepository>>();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetCurrentDirectory()}/../../../../RX.Svng.Web")
                .AddJsonFile("test.ConnectionStrings.json")
                .Build();

            _dbProvider = new DbProvider(configuration);
        }

        [Fact]
        public async Task GetClosestPharmacyWhenValid()
        {
            var testResponse = new PharmacyDataModel
            {
                Name = "CVS PHARMACY",
                Address = "5001 WEST 135 ST",
                Distance = 65.98M
            };

            var pharmacyRepository = new PharmacyRepository(_dbProvider, _loggerMock.Object);
            var result = await pharmacyRepository.FindClosestPharmacyAsync(37.926752574631, -94.66152902993963);

            Assert.Equal(testResponse, result);
        }
    }
}
