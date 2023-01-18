using System.Diagnostics;

namespace CityInfo.Services
{
    public class CloudMailService : IMailService
    {
        private IConfiguration configuration;

        public CloudMailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail From {configuration["mailSettings:_mailFrom"]} to {configuration["mailSettings:_mailTo"]}, with cloudMailService");
            Debug.WriteLine($"subject : {subject}");
            Debug.WriteLine($"Message : {message}");
        }
    }
}
