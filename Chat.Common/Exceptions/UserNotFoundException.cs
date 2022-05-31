namespace Chat.Common.Exceptions
{
    public sealed class UserNotFoundException : BadRequestException
    {
        public UserNotFoundException() : base("User not Found")
        {
        }
    }
}