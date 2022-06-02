using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;

namespace Chat.Core.Abstract
{
    public interface INotificationService
    {
        Task NotifyChat(Guid chatId, MessagesResponseDto message);
    }
}