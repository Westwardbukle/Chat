using System;

namespace Chat.Common.RequestFeatures
{
    public class MessagesParameters : RequestParameters
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public Guid? UserId { get; set; } 
        
        public string? SearchTerm { get; set; }

    }
}