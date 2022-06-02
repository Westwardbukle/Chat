using System;
using System.Threading.Tasks;
using Chat.Core.Abstract;

namespace Chat.Core.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        /*private readonly IMessageService _messageService;
        
        public ChatHub( IMessageService messageService)
        {
            _messageService = messageService;
        }
        
        
        public async Task Send(Guid userId, Guid chatId, string text)
        {
            Clients.
            
            await _messageService.SendMessage(userId, chatId, text);
        }*/
     }
}