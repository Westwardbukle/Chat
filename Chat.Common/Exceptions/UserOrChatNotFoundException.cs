namespace Chat.Common.Exceptions
{
    public sealed class UserOrChatNotFoundException : NotFoundException
    {
        public UserOrChatNotFoundException() : base("User or Chat nor found")
        {
        }
    }
}