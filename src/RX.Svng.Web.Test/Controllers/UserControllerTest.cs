using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RX.Svng.Web.Controllers;
using RX.Svng.Web.Service.Models;
using RX.Svng.Web.Service.Services;
using Xunit;

namespace RX.Svng.Web.Test.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserLoginService> _userLoginServicemock;
        private readonly Mock<IApiValidationService> _apiValidationServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public UserControllerTest()
        {
            _userLoginServicemock = new Mock<IUserLoginService>();
            _apiValidationServiceMock = new Mock<IApiValidationService>();
            _loggerMock = new Mock<ILogger<UserController>>();
        }

        [Fact]
        public async Task LoginWhenServiceThrowsUserNotFoundException()
        {
            var testUserInput = new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            };

            _apiValidationServiceMock.Setup(x => x.ValidateUserInput(It.IsAny<UserInputModel>())).Returns(() => new List<string>());
            _userLoginServicemock.Setup(x => x.LoginAsync(It.IsAny<UserInputModel>())).Throws(new UserNotFoundException("user not found"));

            var userController = new UserController(_apiValidationServiceMock.Object, _userLoginServicemock.Object, _loggerMock.Object);

            var actionResult = await userController.Login(testUserInput);
            Assert.IsAssignableFrom<ActionResult<string>>(actionResult);
            Assert.Equal(401, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task LoginWhenServiceThrowsNotAuthorizedException()
        {
            var testUserInput = new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            };

            _apiValidationServiceMock.Setup(x => x.ValidateUserInput(It.IsAny<UserInputModel>())).Returns(() => new List<string>());
            _userLoginServicemock.Setup(x => x.LoginAsync(It.IsAny<UserInputModel>())).Throws(new NotAuthorizedException("user not authorized"));

            var userController = new UserController(_apiValidationServiceMock.Object, _userLoginServicemock.Object, _loggerMock.Object);

            var actionResult = await userController.Login(testUserInput);
            Assert.IsAssignableFrom<ActionResult<string>>(actionResult);
            Assert.Equal(401, ((ObjectResult)actionResult.Result).StatusCode);

        }

        [Fact]
        public async Task LoginWhenUserInputIsInvalid()
        {
            _apiValidationServiceMock.Setup(x => x.ValidateUserInput(It.IsAny<UserInputModel>())).Returns(() => new List<string>{"username empty or null"});
            _userLoginServicemock.Setup(x => x.LoginAsync(It.IsAny<UserInputModel>())).ReturnsAsync("JWT Token");

            var userController = new UserController(_apiValidationServiceMock.Object, _userLoginServicemock.Object, _loggerMock.Object);

            var actionResult = await userController.Login(null);
            Assert.IsAssignableFrom<ActionResult<string>>(actionResult);
            Assert.Equal(400, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task LoginWhenUncaughtException()
        {
            var testUserInput = new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            };

            _apiValidationServiceMock.Setup(x => x.ValidateUserInput(It.IsAny<UserInputModel>())).Returns(() => new List<string>());
            _userLoginServicemock.Setup(x => x.LoginAsync(It.IsAny<UserInputModel>())).Throws(new Exception());

            var userController = new UserController(_apiValidationServiceMock.Object, _userLoginServicemock.Object, _loggerMock.Object);

            var actionResult = await userController.Login(testUserInput);
            Assert.IsAssignableFrom<ActionResult<string>>(actionResult);
            Assert.Equal(500, ((ObjectResult)actionResult.Result).StatusCode);
        }

        [Fact]
        public async Task LoginWhenAllValid()
        {
            var testUserInput = new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            };

            _apiValidationServiceMock.Setup(x => x.ValidateUserInput(It.IsAny<UserInputModel>())).Returns(() => new List<string>());
            _userLoginServicemock.Setup(x => x.LoginAsync(It.IsAny<UserInputModel>())).ReturnsAsync("JWT Token");

            var userController = new UserController(_apiValidationServiceMock.Object, _userLoginServicemock.Object, _loggerMock.Object);

            var actionResult = await userController.Login(testUserInput);
            Assert.IsAssignableFrom<ActionResult<string>>(actionResult);
            Assert.Equal(200, ((OkObjectResult)actionResult.Result).StatusCode);
        }
    }
}
