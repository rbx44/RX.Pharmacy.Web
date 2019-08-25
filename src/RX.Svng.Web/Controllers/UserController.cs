using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;

namespace RX.Svng.Web.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApiValidationService _validationService;
        private readonly IUserLoginService _userLoginService;
        private readonly ILogger<UserController> _logger;

        public UserController(IApiValidationService validationService, IUserLoginService userLoginService, ILogger<UserController> logger)
        {
            _validationService = validationService;
            _userLoginService = userLoginService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserInputModel userInput)
        {
            _logger.LogDebug("Invoked Login");
            try
            {
                var validateResponse = _validationService.ValidateUserInput(userInput).ToList();
                if (validateResponse.Any())
                    return StatusCode(400, JsonConvert.SerializeObject(validateResponse));

                _logger.LogDebug("Login Success");
                return new OkObjectResult(await _userLoginService.LoginAsync(userInput));
            }
            catch (UserNotFoundException e)
            {
                _logger.LogError("UserNotFoundException", e);
                return StatusCode(401, "Incorrect username or password");
            }
            catch (NotAuthorizedException e)
            {
                _logger.LogError("NotAuthorizedException", e);
                return StatusCode(401, "Incorrect username or password");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception", e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
