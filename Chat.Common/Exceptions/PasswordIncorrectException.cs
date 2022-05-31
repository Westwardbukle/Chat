namespace Chat.Common.Exceptions
{
    public sealed class PasswordIncorrectException : BadRequestException
    {
        public PasswordIncorrectException() : base("Password inccorect")
        {
        }
    }
}