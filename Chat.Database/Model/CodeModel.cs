using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Code;
using Chat.Database.Model.Base;

namespace Chat.Database.Model
{
    [Table("RestoreCodes")]
    public class CodeModel : BaseModel
    {
        [ForeignKey("UserModel")]
        public Guid UserId { get; set; }
        
        [Column("Restore_Code")]
        public string Code { get; set; }
        
        public CodePurpose CodePurpose { get; set; }
        
        public UserModel UserModel { get; set; }
    }
}