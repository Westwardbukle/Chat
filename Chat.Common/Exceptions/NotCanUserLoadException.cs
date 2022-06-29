namespace Chat.Common.Exceptions
{
    public sealed class NotCanUserLoadException : NotFoundException
    {
        public NotCanUserLoadException(string message) : base(message)
        {
        }
    }
}