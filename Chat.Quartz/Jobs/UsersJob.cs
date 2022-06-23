using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Core.ExternalSources.Abstract;
using Chat.Database.AbstractRepository;
using Quartz;

namespace ChatQuartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UsersJob : IJob
    {
        private readonly IRepositoryManager _repository;

        private readonly IUserJobService _userJobService;

        private readonly ISmtpService _smtpService;

        public UsersJob(IRepositoryManager repository, IUserJobService userJobService, ISmtpService smtpService)
        {
            _repository = repository;
            _userJobService = userJobService;
            _smtpService = smtpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _userJobService.LoadFromAPIs();
            
            await _repository.User.CreateUserRangeAsync(users);


            foreach (var user in users)
            {
                await _smtpService.SendEmailAsync(user.Email, "Вы были добавлены в наш суперский чат, смените пароль");
            }

            await _repository.SaveAsync();
        }
        
    }
}