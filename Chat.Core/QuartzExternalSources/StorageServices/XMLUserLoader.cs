using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.Model;

namespace Chat.Core.QuartzExternalSources.StorageServices
{
    public class XMLUserLoader : IUserLoader
    {
        public Task<IEnumerable<UserModel>> GetUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}