using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Chat.Core.ExternalSources.Dto
{
    public class FakerApiUser
    {
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        public string Firstname { get; set; }
    }
}