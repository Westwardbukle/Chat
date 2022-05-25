using System;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    public class MessageModel : BaseModel
    {
        
        public string Text { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid ShatId { get; set; }
        
        public DateTime DispatchTime { get; set; }
        
        
        
    }
}