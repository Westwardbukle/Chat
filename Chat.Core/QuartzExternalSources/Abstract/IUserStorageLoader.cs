using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Core.QuartzExternalSources.Abstract
{
    public interface IUserStorageLoader
    {
        Task<IEnumerable<UserModel>> GetUsers();
    }
}