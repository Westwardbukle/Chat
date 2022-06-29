using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.AbstractRepository;
using Quartz;

namespace ChatQuartz.Jobs
{
    [DisallowConcurrentExecution]
    public class UsersJob : IJob
    {
        private readonly IRepositoryManager _repository;
        private readonly IUserSynchronizer _synchronizer;


        private readonly ISmtpService _smtpService;

        public UsersJob(IRepositoryManager repository, ISmtpService smtpService, IUserSynchronizer synchronizer)
        {
            _repository = repository;
            _smtpService = smtpService;
            _synchronizer = synchronizer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            
            
            
            
            await _synchronizer.Sync();
            
            
            
            
            
            
            //Пример отправки уведомлений пользователям, необходим только специальный сервис для реализации

            //var emails = users.Select(u => u.Email).ToList();

            /*await _smtpService.SendRangeEmailAsync(emails,
                $"Вы были добавлены в наш суперский чат, смените ваш старый пароль для безопасности своего аккаунта");*/
        }
    }
}