using System;

namespace Chat.Common.RequestFeatures
{
    public sealed class ChatsParameters : RequestParameters
    {
        public string? SearchTerm { get; set; }
    }
}