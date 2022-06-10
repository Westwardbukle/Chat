using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    /*[Table("Friend")]*/
    public class FriendModel : BaseModel
    {
        public Guid UserId { get; set; }
        
        public Guid FriendId { get; set; }
        
        public bool Confirmed { get; set; }
        
        
        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; }
    }
}