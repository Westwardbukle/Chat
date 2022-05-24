using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    [Table("Users")]
    public class UserModel : BaseModel
    {
        [Column("user_nickanme")]
        public string Nickname { get; set; }
        
        [Column("date_of_birth")]
        public DateTime DateofBirth { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("password")]
        public string Password { get; set; }
        
        [Column("confirmed")]
        public bool Active { get; set; }
        
        [Column("date_Time_activation")]
        public DateTime? DateTimeActivation { get; set; }
        
        

        public ICollection<CodeModel> CodeModel { get; set; }
        
    }
}