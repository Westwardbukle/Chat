using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;
using Chat.Common.Code;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Model
{
    [Table("Code")]
    [Index(nameof(UserId), nameof(Code), nameof(CodePurpose), IsUnique = true)]
    public class CodeModel : BaseModel
    {
        
        public Guid UserId { get; set; }
        
        public string Code { get; set; }
        
        public DateTime DateExpiration { get; set; }
        
        public CodePurpose CodePurpose { get; set; }
        
        [ForeignKey(nameof(Id))]
        public UserModel UserModel { get; set; }
    }
}