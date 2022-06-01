using System.Threading.Tasks;

namespace Chat.Core.Abstract
{
    public interface ISmtpService
    {
        Task SendEmailAsync(string useremail, string message);
    }
}