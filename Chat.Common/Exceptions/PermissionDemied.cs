namespace Chat.Common.Exceptions
{
    public sealed class PermissionDemied : PermissionDeniedException
    {
        public PermissionDemied() : base("Permission denied")
        {
        }
    }
}