﻿using System.Threading.Tasks;
using Chat.Database.Repository.User;

namespace Chat.Database.AbstractRepository
{
    public interface IRepositoryManager
    {
        IChatRepository Chat { get; }

        IUserRepository User { get; }

        ICodeRepository Code { get; }

        IMessageRepository Message { get; }

        IUserChatRepository UserChat { get; }
        
        IFriendRepository Friend { get; }

        Task SaveAsync();
    }
}