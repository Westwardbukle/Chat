using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.Result;

namespace Chat.Core.Message
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageResponseDto>> SendMessage(Guid userId, Guid chatId, string text);
    }
}