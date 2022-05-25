using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    public class UserModel : BaseModel
    {
        
        public string Nickname { get; set; }
        
        
        public DateTime DateofBirth { get; set; }
        
        
        public string Email { get; set; }
        
        
        public string Password { get; set; }
        
        
        public bool Active { get; set; }
        
        
        public DateTime? DateTimeActivation { get; set; }
        
        

        
        public ICollection<MessageModel> MessageModels { get; set; }
        
        public ICollection<UserChatModel> UserChatModel { get; set; }

        public ICollection<CodeModel> CodeModel { get; set; }
        
    }
}