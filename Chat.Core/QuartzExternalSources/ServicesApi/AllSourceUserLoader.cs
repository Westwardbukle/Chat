﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Core.QuartzExternalSources.ServicesApi
{
    public class AllSourceUserLoader : IUserLoader
    {
        private readonly IEnumerable<IUserApi> _userApis;

        public AllSourceUserLoader(IServiceProvider serviceProvider)
        {
            _userApis = serviceProvider.GetServices(typeof(IUserApi)).Cast<IUserApi>();
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var result = new List<UserModel>();

            var loadTasks = new List<Task<IEnumerable<UserModel>>>();

            foreach (var userApi in _userApis)
            {
                var request =  userApi.SendRequest();

                loadTasks.Add(request);
            }

            foreach (var task in loadTasks)
            {
                var users = await task;
                
                result.AddRange(users);
            }

            return result;
        }
        
    }
}