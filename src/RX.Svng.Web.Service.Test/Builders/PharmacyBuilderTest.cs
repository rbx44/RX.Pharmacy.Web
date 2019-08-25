using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Data.Models;
using RX.Svng.Web.Data.Repositories;
using RX.Svng.Web.Service.Builders;
using RX.Svng.Web.Service.Models;
using Xunit;

namespace RX.Svng.Web.Service.Test.Builders
{
    public class PharmacyBuilderTest
    {
        private readonly Mock<IPharmacyRepository> _pharmacyRepositoryMock;
        private readonly Mock<ILogger<PharmacyBuilder>> _loggerMock;

        public PharmacyBuilderTest()
        {
            _pharmacyRepositoryMock = new Mock<IPharmacyRepository>();
            _loggerMock = new Mock<ILogger<PharmacyBuilder>>();
        }

        [Fact]
        public async Task BuilderResponseWhenRepositoryReturnsValidResponse()
        {
            var pharmacyInputModelMock = new PharmacyInputModel
            {
                Latitude = 23.4,
                Longitude = -3.43
            };

            var pharmacyDataModelMock = new PharmacyDataModel
            {
                Name = "Store1",
                Address = "Address1",
                Distance = 23M
            };

            var pharmacyModelMock = new PharmacyModel
            {
                Name = "Store1",
                Address = "Address1",
                Distance = 23M
            };

            _pharmacyRepositoryMock.Setup(x=> x.FindClosestPharmacyAsync(pharmacyInputModelMock.Latitude, pharmacyInputModelMock.Longitude)).ReturnsAsync(pharmacyDataModelMock);
            var pharmacyBuilder = new PharmacyBuilder(_pharmacyRepositoryMock.Object, _loggerMock.Object);
            Assert.True(pharmacyModelMock.Equals(await pharmacyBuilder.FindClosestAsync(pharmacyInputModelMock)));
        }

        [Fact]
        public async Task BuilderResponseWhenRepositoryReturnsNull()
        {
            var pharmacyInputModelMock = new PharmacyInputModel
            {
                Latitude = 23.4,
                Longitude = -3.43
            };

            _pharmacyRepositoryMock.Setup(x => x.FindClosestPharmacyAsync(pharmacyInputModelMock.Latitude, pharmacyInputModelMock.Longitude)).ReturnsAsync(() => null);
            var pharmacyBuilder = new PharmacyBuilder(_pharmacyRepositoryMock.Object, _loggerMock.Object);
            Assert.Null(await pharmacyBuilder.FindClosestAsync(pharmacyInputModelMock));
        }
    }
}
