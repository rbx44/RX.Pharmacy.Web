using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RX.Svng.Web.Data.Exceptions;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;

namespace RX.Svng.Web.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class PharmacyController : ControllerBase
    {
        private readonly IApiValidationService _validationService;
        private readonly IPharmacyService _pharmacyService;
        private readonly ILogger<PharmacyController> _logger;

        public PharmacyController(IApiValidationService validationService, IPharmacyService pharmacyService, ILogger<PharmacyController> logger)
        {
            _validationService = validationService;
            _pharmacyService = pharmacyService;
            _logger = logger;
        }

        [HttpPost("")]
        public async Task<ActionResult<PharmacyModel>> FindClosestPharmacy([FromBody] PharmacyInputModel pharmacyInput)
        {
            _logger.LogDebug("Invoked FindClosestPharmacy");
            try
            {
                var validateResponse = _validationService.ValidatePharmacyInput(pharmacyInput).ToList();
                if (validateResponse.Any())
                    return StatusCode(400, JsonConvert.SerializeObject(validateResponse));

                _logger.LogDebug("FindClosestPharmacy Success");
                return new OkObjectResult(await _pharmacyService.FindClosestAsync(pharmacyInput));
            }
            catch (SqlNullValueException e)
            {
                _logger.LogError("SqlNullValueException", e);
                return StatusCode(400, "Null value in database");
            }
            catch (SqlValueOutOfRangeException e)
            {
                _logger.LogError("SqlValueOutOfRangeException", e);
                return StatusCode(400, "Invalid value in database");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception", e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
