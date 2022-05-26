using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.Result;
using Chat.Core.Chat;
using Chat.Core.Message;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Message;

namespace Chat.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatService _chatService;
        
        public MessageService
        (
            IMessageRepository messageRepository,
            IChatService chatService
        )
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
        }


        public async Task<ResultContainer<MessageResponseDto>> SendMessage()
        {
            var result = new ResultContainer<MessageResponseDto>();
            
            



            return result;
        }
    }
}