using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Controllers;
using RX.Svng.Web.Data.Exceptions;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;
using Xunit;

namespace RX.Svng.Web.Test.Controllers
{
    public class PharmacyControllerTest
    {
        private readonly Mock<IPharmacyService> _pharmacyServicemock;
        private readonly Mock<IApiValidationService> _apiValidationServiceMock;
        private readonly Mock<ILogger<PharmacyController>> _loggerMock;

        public PharmacyControllerTest()
        {
            _loggerMock = new Mock<ILogger<PharmacyController>>();
            _apiValidationServiceMock = new Mock<IApiValidationService>();
            _pharmacyServicemock = new Mock<IPharmacyService>();
        }

        [Fact]
        public async Task FindClosestWhenServiceThrowsSqlNullValueException()
        {
            var testPharmacyInput = new PharmacyInputModel
            {
               Latitude = 23.3,
               Longitude = -42.9
            };

            _apiValidationServiceMock.Setup(x => x.ValidatePharmacyInput(It.IsAny<PharmacyInputModel>())).Returns(() => new List<string>());
            _pharmacyServicemock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).Throws(new SqlNullValueException("name or address is null"));

            var pharmacyController = new PharmacyController(_apiValidationServiceMock.Object,
                _pharmacyServicemock.Object, _loggerMock.Object);

            var actionResult = await pharmacyController.FindClosestPharmacy(testPharmacyInput);
            Assert.IsAssignableFrom<ActionResult<PharmacyModel>>(actionResult);
            Assert.Equal(400, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task FindClosestWhenServiceThrowsSqlValueOutOfRangeException()
        {
            var testPharmacyInput = new PharmacyInputModel
            {
                Latitude = 23.3,
                Longitude = -42.9
            };

            _apiValidationServiceMock.Setup(x => x.ValidatePharmacyInput(It.IsAny<PharmacyInputModel>())).Returns(() => new List<string>());
            _pharmacyServicemock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).Throws(new SqlValueOutOfRangeException("name or address is out of range"));

            var pharmacyController = new PharmacyController(_apiValidationServiceMock.Object,
                _pharmacyServicemock.Object, _loggerMock.Object);

            var actionResult = await pharmacyController.FindClosestPharmacy(testPharmacyInput);
            Assert.IsAssignableFrom<ActionResult<PharmacyModel>>(actionResult);
            Assert.Equal(400, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task FindClosestWhenServiceThrowsUncaughtException()
        {
            var testPharmacyInput = new PharmacyInputModel
            {
                Latitude = 23.3,
                Longitude = -42.9
            };

            _apiValidationServiceMock.Setup(x => x.ValidatePharmacyInput(It.IsAny<PharmacyInputModel>())).Returns(() => new List<string>());
            _pharmacyServicemock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).Throws(new Exception());

            var pharmacyController = new PharmacyController(_apiValidationServiceMock.Object,
                _pharmacyServicemock.Object, _loggerMock.Object);

            var actionResult = await pharmacyController.FindClosestPharmacy(testPharmacyInput);
            Assert.IsAssignableFrom<ActionResult<PharmacyModel>>(actionResult);
            Assert.Equal(500, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task FindClosestWhenInputIsInvalid()
        {
            _apiValidationServiceMock.Setup(x => x.ValidatePharmacyInput(It.IsAny<PharmacyInputModel>())).Returns(() => new List<string> { "Latitude and Longtitude fields are required" });
            _pharmacyServicemock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).ReturnsAsync(() => new PharmacyModel());

            var pharmacyController = new PharmacyController(_apiValidationServiceMock.Object,
                _pharmacyServicemock.Object, _loggerMock.Object);

            var actionResult = await pharmacyController.FindClosestPharmacy(null);
            Assert.IsAssignableFrom<ActionResult<PharmacyModel>>(actionResult);
            Assert.Equal(400, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task FindClosestWhenAllInvalid()
        {
            var testPharmacyInput = new PharmacyInputModel
            {
                Latitude = 23.3,
                Longitude = -42.9
            };

            _apiValidationServiceMock.Setup(x => x.ValidatePharmacyInput(It.IsAny<PharmacyInputModel>())).Returns(() => new List<string>());
            _pharmacyServicemock.Setup(x => x.FindClosestAsync(It.IsAny<PharmacyInputModel>())).ReturnsAsync(() => new PharmacyModel());

            var pharmacyController = new PharmacyController(_apiValidationServiceMock.Object,
                _pharmacyServicemock.Object, _loggerMock.Object);

            var actionResult = await pharmacyController.FindClosestPharmacy(testPharmacyInput);
            Assert.IsAssignableFrom<ActionResult<PharmacyModel>>(actionResult);
            Assert.Equal(200, ((ObjectResult)actionResult.Result).StatusCode);
        }
    }
}
