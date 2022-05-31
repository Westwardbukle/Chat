using System;

namespace Chat.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        protected NotFoundException(string message):base(message){}
    }
}