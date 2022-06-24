using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Quartz;

namespace ChatQuartz.Jobs
{
    [DisallowConcurrentExecution]
    public class EmailNotifications : IJob
    {
        private readonly IRepositoryManager _repository;
        private readonly ISmtpService _smtpService;

        public EmailNotifications(IRepositoryManager repository, ISmtpService smtpService)
        {
            _repository = repository;
            _smtpService = smtpService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _repository.User.GetAllUsersEmailsByCondition(u => u.Active == false, false);
            
            await _smtpService.SendRangeEmailAsync(users, "Время активировать аккаунт!");
        }
    }
}