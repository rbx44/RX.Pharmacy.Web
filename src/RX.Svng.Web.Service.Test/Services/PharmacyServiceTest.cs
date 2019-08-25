using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Cache.Repositories;
using RX.Svng.Web.Service.Builders;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;
using Xunit;

namespace RX.Svng.Web.Service.Test.Services
{
    public class PharmacyServiceTest
    {
        private readonly Mock<IPharmacyBuilder> _pharmacyBuilderMock;
        private readonly Mock<ICacheRepository> _cacheRepositoryMock;
        private readonly Mock<ILogger<PharmacyService>> _loggerMock;

        public PharmacyServiceTest()
        {
            _pharmacyBuilderMock = new Mock<IPharmacyBuilder>();
            _cacheRepositoryMock = new Mock<ICacheRepository>();
            _loggerMock = new Mock<ILogger<PharmacyService>>();
        }

        [Fact]
        public async Task FindClosestWhenPharmacyInputIsNull()
        {
            _pharmacyBuilderMock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>()))
                .ReturnsAsync(new PharmacyModel());
            _cacheRepositoryMock.Setup(x => x.GetByKeyAsync<PharmacyModel>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new PharmacyModel());

            var pharmacyService = new PharmacyService(_pharmacyBuilderMock.Object, _cacheRepositoryMock.Object,
                _loggerMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await pharmacyService.FindClosestAsync(null));
        }

        [Fact]
        public async Task FindClosestWhenCacheIsNullAndBuilderReturnsNull()
        {
            _pharmacyBuilderMock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).ReturnsAsync(() => null);

            _cacheRepositoryMock.Setup(x => x.GetByKeyAsync<PharmacyModel>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var pharmacyService = new PharmacyService(_pharmacyBuilderMock.Object, _cacheRepositoryMock.Object,
                _loggerMock.Object);

            await pharmacyService.FindClosestAsync(new PharmacyInputModel());

            _pharmacyBuilderMock.Verify(x=> x.FindClosestAsync(It.IsAny<PharmacyInputModel>()), Times.Once);
            _cacheRepositoryMock.Verify(x => x.UpsertKeyAsync(It.IsAny<string>(), It.IsAny<PharmacyModel>()), Times.Never);
        }

        [Fact]
        public async Task FindClosestWhenCacheIsNotNull()
        {
            _pharmacyBuilderMock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>()));

            _cacheRepositoryMock.Setup(x => x.GetByKeyAsync<PharmacyModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new PharmacyModel());

            var pharmacyService = new PharmacyService(_pharmacyBuilderMock.Object, _cacheRepositoryMock.Object,
                _loggerMock.Object);

            await pharmacyService.FindClosestAsync(new PharmacyInputModel());

            _pharmacyBuilderMock.Verify(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>()), Times.Never);
        }

        [Fact]
        public async Task FindClosestWhenCacheIsNullAndBuilderReturnsNotNull()
        {
            _pharmacyBuilderMock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).ReturnsAsync(new PharmacyModel());

            _cacheRepositoryMock.Setup(x => x.GetByKeyAsync<PharmacyModel>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            var pharmacyService = new PharmacyService(_pharmacyBuilderMock.Object, _cacheRepositoryMock.Object,
                _loggerMock.Object);

            await pharmacyService.FindClosestAsync(new PharmacyInputModel());

            _pharmacyBuilderMock.Verify(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>()), Times.Once);
            _cacheRepositoryMock.Verify(x => x.UpsertKeyAsync(It.IsAny<string>(), It.IsAny<PharmacyModel>()), Times.Once);
            _cacheRepositoryMock.Verify(x => x.SetKeyExpirationAsync(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
