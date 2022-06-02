using System;
using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Database.Model;

namespace Chat.Core.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        /*public async Task Send(Guid chatid, MessageModel message)
        {
            Clients.All.SendCoreAsync("Receive", $"{chatid}")
        }*/
    }
}