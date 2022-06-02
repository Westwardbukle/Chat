using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Abstract;
using Chat.Core.Hub;

namespace Chat.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IAuthService _authService;
        private readonly IChatHub _hub;
        public NotificationService(IAuthService authService, IChatHub hub )
        {
            _authService = authService;
            _hub = hub;
        }

        public Task NotifyChat(Guid chatId, MessagesResponseDto message)
        {
           var users = _authService.GetAllUsersInChat(chatId);

           //_hub.Send()
            
            // TODO: Уведомить все юзеров в chatId
            
            throw new NotImplementedException();
        }
    }
    
}