using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Core.Abstract;
using Chat.Core.Hubs;
using Chat.Database.Repository.Manager;

namespace Chat.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryManager _repository;
        private readonly IChatWatcher _chatWatcher;

        public NotificationService(IChatWatcher chatWatcher, IRepositoryManager repository)
        {
            _chatWatcher = chatWatcher;
            _repository = repository;
        }

        public async Task NotifyChat(Guid chatId, MessagesResponseDto message)
        {
            var usersIds = _repository.User.GetAllUsersInChat(chatId).Select(x => x.Id).ToList();

            await _chatWatcher.NotifyNewMessageChat(usersIds, message);
        }
    }
}