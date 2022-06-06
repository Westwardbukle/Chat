using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Core.Hubs
{
    public class ChatWatcher : IChatWatcher
    {
        private readonly IHubContext<ChatHub> _hubContext;
        
        public ChatWatcher(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNewMessageChat(List<Guid> ids, MessagesResponseDto message)
        {
            var groups = ids.Select(x => x.ToString());

            await _hubContext.Clients.Groups(groups).SendAsync("ChatNewMessage", message);
        }
    }
}