using System;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    public class Friend : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        
    }
}