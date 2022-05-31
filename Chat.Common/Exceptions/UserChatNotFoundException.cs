namespace Chat.Common.Exceptions
{
    public sealed class UserChatNotFoundException : NotFoundException
    {
        public UserChatNotFoundException() : base("The user is not in the chat")
        {
        }
    }
}