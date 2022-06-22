using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.User;
using Chat.Database.Model;

namespace Chat.Core.ExternalSources.Abstract
{
    public interface IUserApi
    {
        Task<List<UserModel>> SendRequest();
    }
}