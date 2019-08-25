using System.Collections.Generic;
using RX.Svng.Web.Service.Models;

namespace RX.Svng.Web.Service.Services
{
    public interface IApiValidationService
    {
        IEnumerable<string> ValidateUserInput(UserInputModel userInput);
        IEnumerable<string> ValidatePharmacyInput(PharmacyInputModel pharmacyInput);
    }
    public class ApiValidationService : IApiValidationService
    {
        public IEnumerable<string> ValidateUserInput(UserInputModel userInput)
        {
            if (userInput == null)
                yield return "Username and Password fields are required";

            if (userInput != null && string.IsNullOrEmpty(userInput.Username))
                yield return "Username field is required";

            if (userInput != null && string.IsNullOrEmpty(userInput.Password))
                yield return "Password field is required";
        }

        public IEnumerable<string> ValidatePharmacyInput(PharmacyInputModel pharmacyInput)
        {
            if (pharmacyInput == null)
                yield return "Latitude and Longtitude fields are required";
        }
    }
}
