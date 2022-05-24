using System.Threading.Tasks;
using Chat.Core.Smtp;
using Chat.Core.Validating;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Chat.Core.Services
{
    public class SmtpSevice : ISmtpService
    {
        private readonly string _configurationEmail;
        private readonly string _configurationPassword;
        private readonly string _configurationSmtp;
        private readonly int _configurationPort;

        public SmtpSevice
        (
            IConfiguration configuration
        )
        {
            _configurationEmail = configuration.GetValue<string>("Email");
            _configurationPassword = configuration.GetValue<string>("Password");
            _configurationSmtp = configuration.GetValue<string>("Smtp");
            _configurationPort = configuration.GetValue<int>("PortSmtp");
        }
        
        public async Task SendEmailAsync(string useremail, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Chat",_configurationEmail));
            emailMessage.To.Add(new MailboxAddress("" , useremail));
            emailMessage.Subject = Consts.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
           using var client = new SmtpClient();
             await client.ConnectAsync(_configurationSmtp, _configurationPort, true);
             await  client.AuthenticateAsync(_configurationEmail, _configurationPassword);
             await client.SendAsync(emailMessage);
             await client.DisconnectAsync(true);
        }
        
    }
}