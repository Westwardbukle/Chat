using System;
using System.Collections.Generic;
using Chat.Database.Model;
using Chat.Database.Repository.Base;
using Chat.Database.Repository.User;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository.UserChat
{
    public class UserChatRepository : BaseRepository<UserChatModel>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}