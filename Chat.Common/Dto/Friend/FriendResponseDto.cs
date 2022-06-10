using System;

namespace Chat.Common.Dto.Friend
{
    public class FriendResponseDto
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid FriendId { get; set; }
        
        public bool Confirmed { get; set; }
        
        public DateTime DateCreated { get; set; }
        
    }
}