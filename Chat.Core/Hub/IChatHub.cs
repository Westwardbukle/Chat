using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Chat.Database.Model;
using MailKit;

namespace Chat.Core.Hub
{
    public interface IChatHub
    {
        Task Send(List<Guid> ids, MessagesResponseDto message);
    }
}