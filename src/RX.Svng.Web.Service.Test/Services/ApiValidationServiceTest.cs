using System.Linq;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;
using Xunit;

namespace RX.Svng.Web.Service.Test.Services
{
    public class ApiValidationServiceTest
    {
        private readonly ApiValidationService _apiValidationService;

        public ApiValidationServiceTest()
        {
            _apiValidationService = new ApiValidationService();
        }

        [Fact]
        public void ValidateUserInputWhenUserInputIsNull()
        {
            Assert.Single(_apiValidationService.ValidateUserInput(null));
        }

        [Fact]
        public void ValidateUserInputWhenUserInputIsInvalid()
        {
            var testInput = new UserInputModel();
            Assert.Equal(2, _apiValidationService.ValidateUserInput(testInput).Count());
        }

        [Fact]
        public void ValidateUserInputWhenUserInputIsValid()
        {
            var testInput = new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            };

            Assert.Empty(_apiValidationService.ValidateUserInput(testInput));
        }

        [Fact]
        public void ValidatePharmacyInputWhenPharmacyInputModelIsNull()
        {
            Assert.Single(_apiValidationService.ValidatePharmacyInput(null));
        }

        [Fact]
        public void ValidatePharmacyInputWhenPharmacyInputIsValid()
        {
            var pharmacyInputModel = new PharmacyInputModel
            {
                Latitude = 23.3,
                Longitude = -42.3
            };
            Assert.Empty(_apiValidationService.ValidatePharmacyInput(pharmacyInputModel));
        }
    }
}
