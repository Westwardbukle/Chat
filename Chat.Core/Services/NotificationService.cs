using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Abstract;

namespace Chat.Core.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService()
        {
        }

        public Task NotifyChat(Guid chatId, MessagesResponseDto message)
        {
            
            
            // TODO: Уведомить все юзеров в chatId
            
            throw new NotImplementedException();
        }
    }
    
}