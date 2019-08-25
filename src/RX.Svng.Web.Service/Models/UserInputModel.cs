using System;

namespace RX.Svng.Web.Service.Models
{
    public class UserInputModel
    {
        private string _username;
        private string _password;
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException($"Username cannot be null");

                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException($"Password cannot be null");

                _password = value;
            }
        }
    }
}
