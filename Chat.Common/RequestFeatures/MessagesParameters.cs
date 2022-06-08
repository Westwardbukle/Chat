using System;

namespace Chat.Common.RequestFeatures
{
    public class MessagesParameters : RequestParameters
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public bool ValidDateRange => MaxDate > MinDate;
    }
}