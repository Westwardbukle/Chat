using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Chat.Common.Base;
using Chat.Common.Code;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Model
{
    [Table("RestoreCodes")]
    [Index(nameof(UserModelId), nameof(Code), nameof(CodePurpose), IsUnique = true)]
    public class CodeModel : BaseModel
    {
        
        public Guid UserModelId { get; set; }
        
        public string Code { get; set; }
        
        public DateTime DateExpiration { get; set; }
        
        public CodePurpose CodePurpose { get; set; }
        
        
        
        public UserModel UserModel { get; set; }
    }
}