using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Message
{
    public interface IMessageService
    {
        Task<ActionResult> SendMessage(Guid userId, Guid chatId, string text);
    }
}