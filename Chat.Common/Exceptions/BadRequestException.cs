using System;

namespace Chat.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        protected BadRequestException(string message) : base(message)
        {
        }
    }
}