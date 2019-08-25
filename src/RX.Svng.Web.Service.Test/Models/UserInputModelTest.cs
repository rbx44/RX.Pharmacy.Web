using System;
using RX.Svng.Web.Service.Models;
using Xunit;

namespace RX.Svng.Web.Service.Test.Models
{
    public class UserInputModelTest
    {
        [Fact]
        public void UsernameIsEmptyWhenPasswordIsSet()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = "",
                Password = "Password1"
            });
        }

        [Fact]
        public void UsernameIsNullWhenPasswordIsSet()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = "",
                Password = "Password1"
            });
        }

        [Fact]
        public void PasswordIsEmptyWhenUsernameIsSet()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = "User1",
                Password = ""
            });
        }

        [Fact]
        public void PasswordIsNullWhenUsernameIsSet()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = "User1",
                Password = null
            });
        }

        [Fact]
        public void PasswordAndUserNameEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = "",
                Password = ""
            });
        }

        [Fact]
        public void PasswordAndUserNameNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UserInputModel
            {
                Username = null,
                Password = null
            });
        }

        [Fact]
        public void PasswordAndUserNameValid()
        {
            Assert.IsType<UserInputModel>(new UserInputModel
            {
                Username = "User1",
                Password = "Password1"
            });
        }
    }
}
