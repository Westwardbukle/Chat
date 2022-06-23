using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Quartz;

namespace ChatQuartz.Jobs
{
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
           var users = await _repository.User.GetAllUsersbyCondition(u => u.Active == false, false);

           foreach (var user in users)
           {
               await _smtpService.SendEmailAsync(user.Email, "Время активировать почту!");
           }
        }
    }
}