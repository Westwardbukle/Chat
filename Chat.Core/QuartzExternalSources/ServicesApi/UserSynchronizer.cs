using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Core.QuartzExternalSources.ServicesApi
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

            var unconfirmedUsers = new List<UserModel>();

            foreach (var user in users)
            {
                if (await _repository.User.GetUserAsync(u => u.Nickname == user.Nickname || u.Email == user.Email) is
                    null)
                {
                    if (user.Nickname != "")
                    {
                        unconfirmedUsers.Add(user);
                    }
                }
            }

            await _repository.User.CreateUserRangeAsync(unconfirmedUsers);

            await _repository.SaveAsync();
        }
    }
}