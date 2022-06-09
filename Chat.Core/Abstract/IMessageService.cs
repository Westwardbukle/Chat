﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Common.RequestFeatures;
using Chat.Database.Model;

namespace Chat.Core.Abstract
{
    public interface IMessageService
    {
        Task<MessagesResponseDto> SendMessage(Guid userId, Guid chatId, string text);

        
        Task<MessagesResponseDto> SendPersonalMessage(Guid senderId, Guid recipientId, string text);

        Task<(List<MessagesResponseDto> Data, MetaData MetaData )> GetAllMessagesFromUserToUser(Guid userId,
            Guid senderId, MessagesParameters messagesParameters);
    }
}