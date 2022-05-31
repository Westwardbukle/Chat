namespace Chat.Common.Exceptions
{
    public sealed class ChatNotFoundException : NotFoundException
    {
        public ChatNotFoundException() : base("Chat not found")
        {}
    }
}