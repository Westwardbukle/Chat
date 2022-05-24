using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Database.Model.Base;

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
        
        [Column("Confirmed")]
        public bool Active { get; set; }

    }
}