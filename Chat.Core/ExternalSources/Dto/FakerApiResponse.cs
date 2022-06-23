using System.Collections.Generic;

namespace Chat.Core.ExternalSources.Dto
{
    public class FakerApiResponse
    {
        public string Status { get; set; }
        
        public int Code { get; set; }
        
        public int Total { get; set; }
        
        public List<FakerApiUser> Data { get; set; }
    }
}