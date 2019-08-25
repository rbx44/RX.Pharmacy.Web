using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NeoSmart.AsyncLock;
using RX.Svng.Web.Cache.Repositories;
using RX.Svng.Web.Service.Builders;
using RX.Svng.Web.Service.Models;

namespace RX.Svng.Web.Service.Services
{
    public interface IPharmacyService
    {
        Task<PharmacyModel> FindClosestAsync(PharmacyInputModel pharmacyInput);
    }

    public class PharmacyService : IPharmacyService
    {
        private readonly IPharmacyBuilder _pharmacyBuilder;
        private readonly ICacheRepository _cacheRepository;
        private readonly ILogger<PharmacyService> _logger;
        private readonly AsyncLock _lock = new AsyncLock();
        private const string Payload = "payload";

        public PharmacyService(IPharmacyBuilder pharmacyBuilder, ICacheRepository cacheRepository, ILogger<PharmacyService> logger)
        {
            _pharmacyBuilder = pharmacyBuilder;
            _cacheRepository = cacheRepository;
            _logger = logger;
        }

        public async Task<PharmacyModel> FindClosestAsync(PharmacyInputModel pharmacyInput)
        {
            _logger.LogDebug("Invoked Service FindClosestAsync");

            if (pharmacyInput == null)
                throw new ArgumentNullException();

            var key = $"pharmacy:{pharmacyInput.Latitude}:{pharmacyInput.Longitude}";

            _logger.LogDebug($"Findind closest pharmacy to lat [{pharmacyInput.Latitude}] and long [{pharmacyInput.Longitude}]");
            var firstCheck = await _cacheRepository.GetByKeyAsync<PharmacyModel>(Payload, key);
            if (firstCheck != null)
            {
                _logger.LogDebug("Service first cache successful");
                return firstCheck;
            }

            _logger.LogDebug("Service first cache miss");
            using (await _lock.LockAsync())
            {
                var secondCheck = await _cacheRepository.GetByKeyAsync<PharmacyModel>(Payload, key);
                if (secondCheck != null)
                {
                    _logger.LogDebug("Service second cache successful");
                    return secondCheck;
                }

                _logger.LogDebug("Service second cache miss");
                var pharmacy = await _pharmacyBuilder.FindClosestAsync(pharmacyInput);
                if (pharmacy == null)
                {
                    _logger.LogWarning("Service pharmacy closest null from builder, returning empty object success");
                    _logger.LogDebug("Service FindClosestAsync empty successful");
                    return new PharmacyModel();
                }

                await _cacheRepository.UpsertKeyAsync(key, pharmacy);
                await _cacheRepository.SetKeyExpirationAsync(key, TimeSpan.FromHours(2));

                _logger.LogDebug("Service FindClosestAsync successful");
                return pharmacy;
            }
        }
    }
}
