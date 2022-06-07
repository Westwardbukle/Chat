namespace Chat.Common.Exceptions
{
    public sealed class YouAreNotAdministratorException : PermissionDeniedException
    {
        public YouAreNotAdministratorException() : base("You are not a chat administrator")
        {
        }
    }
}