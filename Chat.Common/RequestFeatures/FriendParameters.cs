using System;

namespace Chat.Common.RequestFeatures
{
    public class FriendParameters : RequestParameters
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        
        public bool? ActiveTerm { get; set; }
    }
}