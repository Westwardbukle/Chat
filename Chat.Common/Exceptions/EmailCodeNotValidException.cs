namespace Chat.Common.Exceptions
{
    public sealed class EmailCodeNotValidException : BadRequestException
    {
        public EmailCodeNotValidException() : base("Verification code is not valid")
        {
        }
    }
}