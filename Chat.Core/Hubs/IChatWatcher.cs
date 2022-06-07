using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;

namespace Chat.Core.Hubs
{
    public interface IChatWatcher
    {
        Task NotifyNewMessageChat(List<Guid> ids, MessagesResponseDto message);
    }
}