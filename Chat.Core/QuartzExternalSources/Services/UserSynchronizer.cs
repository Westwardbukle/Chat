using System.Threading.Tasks;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.AbstractRepository;

namespace Chat.Core.QuartzExternalSources.Services
{
    public class UserSynchronizer : IUserSynchronizer
    {
        private readonly IUserLoader _userLoader;
        private readonly IRepositoryManager _repository;

        public UserSynchronizer(IRepositoryManager repository, IUserLoader userLoader)
        {
            _repository = repository;
            _userLoader = userLoader;
        }

        public async Task Sync()
        {
            var users = await _userLoader.GetUsers();
            
            await _repository.User.CreateUserRangeAsync(users);
            await _repository.SaveAsync();
        }
    }
}