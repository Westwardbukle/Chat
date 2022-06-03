using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Abstract;
using Chat.Core.Hub;
using Chat.Database.Repository.Manager;

namespace Chat.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IChatHub _hub;
        private readonly IRepositoryManager _repository;
        public NotificationService( IChatHub hub, IRepositoryManager repository)
        {
            _hub = hub;
            _repository = repository;
        }

        public async Task NotifyChat(Guid chatId, MessagesResponseDto message)
        {
            var usersIds = _repository.User.GetAllUsersInChat(chatId).Select(x => x.Id).ToList();

            await _hub.Send(usersIds, message);
        }
        
        
        
        
        
    }
    
}