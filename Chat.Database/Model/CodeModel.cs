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
        
        [Column("restore_code")]
        public string Code { get; set; }
        
        [Column("date_expiration")]
        public DateTime DateExpiration { get; set; }
        
        [Column("code_purpose")]
        public CodePurpose CodePurpose { get; set; }
        
        
        
        public UserModel UserModel { get; set; }
    }
}