using System;
using System.Threading.Tasks;
using Chat.Common.Validating;
using Chat.Core.Smtp;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Chat.Core.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly string _configurationEmail;
        private readonly string _configurationPassword;
        private readonly string _configurationSmtp;
        private readonly int _configurationPort;

        public SmtpService(IConfiguration configuration)
        {
            _configurationEmail = configuration.GetValue<string>("Email");
            _configurationPassword = configuration.GetValue<string>("Password");
            _configurationSmtp = configuration.GetValue<string>("Smtp");
            _configurationPort = configuration.GetValue<int>("PortSmtp");
        }
        
        public async Task SendEmailAsync(string userEmail, string message)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(new MailboxAddress("Chat",_configurationEmail));
            emailMessage.To.Add(new MailboxAddress("" , userEmail));
            emailMessage.Subject = Consts.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            
            using var client = new SmtpClient();
            
            await client.ConnectAsync(_configurationSmtp, _configurationPort, true);
            await client.AuthenticateAsync(_configurationEmail, _configurationPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        
    }
}