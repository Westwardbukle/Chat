﻿using System;
using System.Threading.Tasks;
using Chat.Database.Repository.Chat;
using Chat.Database.Repository.Code;
using Chat.Database.Repository.Message;
using Chat.Database.Repository.User;
using Chat.Database.Repository.UserChat;

namespace Chat.Database.Repository.Manager
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _appDbContext;
        private readonly Lazy<IChatRepository> _chatRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IMessageRepository> _messageRepository;
        private readonly Lazy<ICodeRepository> _codeRepository;
        private readonly Lazy<IUserChatRepository> _userChatRepository;

        public RepositoryManager(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _chatRepository = new Lazy<IChatRepository>(() => new ChatRepository(appDbContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(appDbContext));
            _codeRepository = new Lazy<ICodeRepository>(() => new CodeRepository(appDbContext));
            _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(appDbContext));
            _userChatRepository = new Lazy<IUserChatRepository>(() => new UserChatRepository(appDbContext));
        }

        public IChatRepository Chat => _chatRepository.Value;
        public IUserRepository User => _userRepository.Value;
        public ICodeRepository Code => _codeRepository.Value;
        public IMessageRepository Message => _messageRepository.Value;
        public IUserChatRepository UserChat => _userChatRepository.Value;

        public async Task SaveAsync() => await _appDbContext.SaveChangesAsync();
    }
}