namespace Chat.Common.Exceptions
{
    public sealed class EmailUserExist : BadRequestException
    {
        public EmailUserExist() : base("A user with this e-mail already exists")
        {
        }
    }
}