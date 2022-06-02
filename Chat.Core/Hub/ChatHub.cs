using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Database.Model;

namespace Chat.Core.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub, IChatHub
    {
        public async Task Send(List<Guid> ids, MessagesResponseDto message)
        {
            MessagesResponseDto[] messages = new[] { message };
            foreach (var id in ids)
            {
                await Clients.Group(id.ToString()).SendCoreAsync("Receive", messages);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name ?? string.Empty;

            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.User.Identity.Name ?? string.Empty;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, name);
            
            await base.OnDisconnectedAsync(exception);
        }
        
    }
}