using System;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    public class FriendRequestModel : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public bool Confirmed { get; set; }
    }
}