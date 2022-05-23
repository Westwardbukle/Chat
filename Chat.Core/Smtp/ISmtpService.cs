using System.Threading.Tasks;

namespace Chat.Core.Smtp
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string useremail, string message);
    }
}