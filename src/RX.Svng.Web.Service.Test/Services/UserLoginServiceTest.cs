using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Service.Configs;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;
using Xunit;

namespace RX.Svng.Web.Service.Test.Services
{
    public class UserLoginServiceTest
    {
        private readonly Mock<IAwsCredentialsConfiguration> _awsCredentialsConfigurationMock;
        private readonly Mock<ILogger<UserLoginService>> _loggerMock;

        public UserLoginServiceTest()
        {
            _awsCredentialsConfigurationMock = new Mock<IAwsCredentialsConfiguration>();
            _loggerMock = new Mock<ILogger<UserLoginService>>();
        }

        [Fact]
        public async Task LoginAsyncWhenUserInputIsNull()
        {
            _awsCredentialsConfigurationMock.SetupAllProperties();
            var userLoginService = new UserLoginService(_awsCredentialsConfigurationMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userLoginService.LoginAsync(null));
        }

        [Fact]
        public async Task LoginAsyncWhenConfigurationIsNull()
        {
            var userLoginService = new UserLoginService(null, _loggerMock.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userLoginService.LoginAsync(null));
        }

        [Fact]
        public async Task LoginAsyncWhenConfigurationPropertiesIsNull()
        {
            var userLoginService = new UserLoginService(_awsCredentialsConfigurationMock.Object, _loggerMock.Object);
            await Assert.ThrowsAsync<ApplicationException>(async () => await userLoginService.LoginAsync(new UserInputModel()));
        }

        [Fact]
        public async Task LoginAsyncWhenUserInputPropertiesIsNull()
        {
            _awsCredentialsConfigurationMock
                .SetupGet(x => x.UserPoolAppClientId).Returns("PoolClient");
            _awsCredentialsConfigurationMock
                .SetupGet(x => x.AccessKeyId).Returns("AccessKey");
            _awsCredentialsConfigurationMock
                .SetupGet(x => x.Region).Returns("Region");
            _awsCredentialsConfigurationMock
                .SetupGet(x => x.SecretAccessKeyPassword).Returns("Secret");
            _awsCredentialsConfigurationMock
                .SetupGet(x => x.UserPoolId).Returns("UserPool");

            var userLoginService = new UserLoginService(_awsCredentialsConfigurationMock.Object, _loggerMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userLoginService.LoginAsync(new UserInputModel()));
        }
    }
}
