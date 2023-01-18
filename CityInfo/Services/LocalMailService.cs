using System.Diagnostics;

namespace CityInfo.Services
{
    public class LocalMailService : IMailService
    {
        private IConfiguration configuration;

        public LocalMailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //private string _mailTo = "admin@mycompany.com";
        //private string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail From {configuration["mailSettings:_mailFrom"]} to {configuration["mailSettings:_mailTo"]}, with LocalMailService");
            Debug.WriteLine($"subject : {subject}");
            Debug.WriteLine($"Message : {message}");
        }
    }
}
