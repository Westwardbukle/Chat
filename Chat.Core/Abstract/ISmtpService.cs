using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Core.Abstract
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string useremail, string message);
        Task SendRangeEmailAsync(List<string> emails, string message);
    }
}