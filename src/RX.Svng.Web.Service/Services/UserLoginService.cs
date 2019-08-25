using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Logging;
using RX.Svng.Web.Service.Configs;
using RX.Svng.Web.Service.Models;

namespace RX.Svng.Web.Service.Services
{
    public interface IUserLoginService
    {
        Task<string> LoginAsync(UserInputModel userInput);
    }
    public class UserLoginService : IUserLoginService
    {
        private readonly IAwsCredentialsConfiguration _credentialsConfiguration;
        private readonly ILogger<UserLoginService> _logger;

        public UserLoginService(IAwsCredentialsConfiguration credentialsConfiguration, ILogger<UserLoginService> logger)
        {
            _credentialsConfiguration = credentialsConfiguration;
            _logger = logger;
        }

        public async Task<string> LoginAsync(UserInputModel userInput)
        {
            _logger.LogDebug("Invoked Service LoginAsync");

            if(userInput == null)
                throw new ArgumentNullException($"userInput is null");

            if(_credentialsConfiguration == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(_credentialsConfiguration.AccessKeyId) || string.IsNullOrEmpty(_credentialsConfiguration.SecretAccessKeyPassword) || string.IsNullOrEmpty(_credentialsConfiguration.Region))
                throw new ApplicationException();

            if (string.IsNullOrEmpty(userInput.Username) || string.IsNullOrEmpty(userInput.Password))
                throw new ArgumentNullException($"username or password null");

            var client = new AmazonCognitoIdentityProviderClient(
                _credentialsConfiguration.AccessKeyId,
                _credentialsConfiguration.SecretAccessKeyPassword,
                RegionEndpoint.GetBySystemName(_credentialsConfiguration.Region)
            );

            _logger.LogDebug($"Service LoginAsync logging in for user: [{userInput.Username}]");
            var session = await client.AdminInitiateAuthAsync(new AdminInitiateAuthRequest
            {
                UserPoolId = _credentialsConfiguration.UserPoolId,
                ClientId = _credentialsConfiguration.UserPoolAppClientId,
                AuthFlow = "ADMIN_NO_SRP_AUTH",
                AuthParameters = new Dictionary<string, string>
                {
                    {"USERNAME", userInput.Username},
                    {"PASSWORD", userInput.Password}
                }
            });

            _logger.LogDebug("Service LoginAsync successful");

            return session.AuthenticationResult.IdToken;
        }
    }
}
