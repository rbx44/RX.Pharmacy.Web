using System;

namespace RX.Svng.Web.Service.Configs
{
    public interface IAwsCredentialsConfiguration
    {
        string AccessKeyId { get; }
        string Region { get; }
        string SecretAccessKeyPassword { get; }
        string UserPoolAppClientId { get; }
        string UserPoolId { get; }
    }

    public class AwsCredentialsConfiguration : IAwsCredentialsConfiguration
    {
        public string AccessKeyId => Environment.GetEnvironmentVariable("APPSETTING_AccessKeyId", EnvironmentVariableTarget.User) ?? throw new ApplicationException("missing required app settings");
        public string SecretAccessKeyPassword => Environment.GetEnvironmentVariable("APPSETTING_SecretAccessKey", EnvironmentVariableTarget.User) ?? throw new ApplicationException("missing required app settings");
        public string UserPoolId => Environment.GetEnvironmentVariable("APPSETTING_UserPoolId", EnvironmentVariableTarget.User) ?? throw new ApplicationException("missing required app settings");
        public string UserPoolAppClientId => Environment.GetEnvironmentVariable("APPSETTING_UserPoolAppClientId", EnvironmentVariableTarget.User) ?? throw new ApplicationException("missing required app settings");
        public string Region => Environment.GetEnvironmentVariable("APPSETTING_Region", EnvironmentVariableTarget.User) ?? throw new ApplicationException("missing required app settings");
    }
}
