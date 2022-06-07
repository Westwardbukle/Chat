using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.RequestFeatures;

namespace Chat.Core.Abstract
{
    public interface INotificationService
    {
        Task NotifyChat(Guid chatId, MessagesResponseDto message);
    }
}