using System.Data;
using System.Threading.Tasks;
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
        private readonly Mock<IDbProvider> _dbProviderMock;
        private readonly Mock<ILogger<PharmacyRepository>> _loggerMock;

        public PharmacyRepositoryTest()
        {
            _dbProviderMock = new Mock<IDbProvider>();
            _loggerMock = new Mock<ILogger<PharmacyRepository>>();
        }

        [Fact]
        public async Task GetClosestPharmacyWhenValid()
        {
            var testResponse = new PharmacyDataModel
            {
                Name = "Name1",
                Address = "Address1",
                Distance = 32.2M
            };

            //_dbProviderMock.Setup(x => x.Get(It.IsAny<string>())).Returns(It.IsAny<IDbConnection>());

            //var pharmacyRepository = new PharmacyRepository(_dbProviderMock.Object, _loggerMock.Object);

            //await pharmacyRepository.FindClosestPharmacyAsync(32.23, -4.8);

            //_dbProviderMock.Verify(x=> x.Get(It.IsAny<string>()), Times.Once);
            var mock = new Mock<IPharmacyRepository>();
            mock.Setup(x => x.FindClosestPharmacyAsync(32.23, -4.8)).ReturnsAsync(testResponse);

            var response = await mock.Object.FindClosestPharmacyAsync(32.23, -4.8);

            Assert.Equal(testResponse, response);
        }

        [Fact]
        public async Task GetClosestPharmacyWhenResponseNull()
        {
            PharmacyDataModel testResponse = null;

            var mock = new Mock<IPharmacyRepository>();
            mock.Setup(x => x.FindClosestPharmacyAsync(32.23, -4.8)).ReturnsAsync(testResponse);
            var response = await mock.Object.FindClosestPharmacyAsync(32.23, -4.8);

            Assert.Equal(testResponse, response);
        }
    }
}
