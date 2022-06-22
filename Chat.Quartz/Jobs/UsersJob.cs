using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Core.Abstract;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.ExternalSources.Services;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace ChatQuartz.Jobs
{
    //[DisallowConcurrentExecution]
    public class UsersJob : IJob
    {

        private readonly IUserService _userService;

        private readonly IServiceProvider _serviceProvider;

        public UsersJob(IUserService userService, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var userService = new UserService(_serviceProvider);

            var result = await userService.LoadFromAPIs();
            
        }
    }
}