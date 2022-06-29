using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.Model;

namespace Chat.Core.QuartzExternalSources.StorageServices
{
    public class CSVUserLoader : IUserLoader
    {
        public Task<IEnumerable<UserModel>> GetUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}