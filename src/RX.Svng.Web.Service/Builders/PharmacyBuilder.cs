using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RX.Svng.Web.Data.Repositories;
using RX.Svng.Web.Service.Models;

namespace RX.Svng.Web.Service.Builders
{
    public interface IPharmacyBuilder
    {
        Task<PharmacyModel> FindClosestAsync(PharmacyInputModel pharmacyInput);
    }

    public class PharmacyBuilder : IPharmacyBuilder
    {
        private readonly IPharmacyRepository _pharmacyRepository;
        private readonly ILogger<PharmacyBuilder> _logger;

        public PharmacyBuilder(IPharmacyRepository pharmacyRepository, ILogger<PharmacyBuilder> logger)
        {
            _pharmacyRepository = pharmacyRepository;
            _logger = logger;
        }

        public async Task<PharmacyModel> FindClosestAsync(PharmacyInputModel pharmacyInput)
        {
            _logger.LogDebug("Invoked Builder FindClosestAsync");
            var data = await _pharmacyRepository.FindClosestPharmacyAsync(pharmacyInput.Latitude, pharmacyInput.Longitude);

            if (data == null)
            {
                _logger.LogDebug("Builder null FindClosestAsync successful");
                return null;
            }

            _logger.LogDebug("Builder FindClosestAsync successful");
            return new PharmacyModel
            {
                Name = data.Name,
                Address = data.Address,
                Distance = data.Distance
            };
        }
    }
}
