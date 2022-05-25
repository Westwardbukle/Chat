using Chat.Common.Base;
using Chat.Common.UsersRole;

namespace Chat.Database.Model
{
    public class UserChatModel : BaseModel
    {
        public UserModel User { get; set; }
        
        public ChatModel Chat { get; set; }
        
        public Role Role { get; set; }
    }
}