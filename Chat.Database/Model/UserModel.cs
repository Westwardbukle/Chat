using Chat.Database.Model.Base;

namespace Chat.Database.Model
{
    public class UserModel : BaseModel
    {
        public string Nickname { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
}