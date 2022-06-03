using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Core.Hub
{
    [Authorize]
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub, IChatHub
    {
        public async Task Send(List<Guid> ids, MessagesResponseDto message/*, string chatName*/ )
        {
            var convertIds = ids.Select(x => x.ToString());
            
            await Clients.Groups(convertIds).SendAsync("Send", message);;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new Exception("пошел нгазхуй");
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                throw new Exception("пошел нгазхуй");
            }
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            
            await base.OnDisconnectedAsync(exception);
        }
        
    }
}