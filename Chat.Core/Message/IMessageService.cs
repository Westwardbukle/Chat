using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Message
{
    public interface IMessageService
    {
        Task<ActionResult> SendMessage(Guid userId, Guid chatId, string text);
        Task<ActionResult> GetAllMessageInChat(Guid chatId);
        /*Task<ActionResult> SendPersonalMessage(Guid senderId, Guid recipientId, string text);*/
        /*Task<ActionResult> GetAllMessagesFromUser();*/
        
    }
}