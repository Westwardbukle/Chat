using System;

namespace Chat.Common.Exceptions
{
    public class PermissionDeniedException : Exception
    {
        protected PermissionDeniedException(string message):base(message){}
    }
}