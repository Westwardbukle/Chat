namespace Chat.Common.Exceptions
{
    public sealed class UserExistException : BadRequestException 
    {
        public UserExistException() : base("User already exist")
        {
        }
    }
}