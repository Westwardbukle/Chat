namespace Chat.Common.Exceptions
{
    public sealed class IncorrectUserException : PermissionDeniedException
    {
        public IncorrectUserException() : base("Permission denied")
        {
        }
    }
}