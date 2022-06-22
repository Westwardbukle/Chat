using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Chat.Core.ExternalSources.Abstract;
using Chat.Database.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Core.ExternalSources.Services
{
    public class UserService
    {

        private readonly IEnumerable<IUserApi> _userApis;

        public UserService(IServiceProvider serviceProvider)
        {
            _userApis = serviceProvider.GetServices(typeof(IUserApi)).Cast<IUserApi>();
        }

        public async Task<List<UserModel>> LoadFromAPIs()
        {
            var result = new List<UserModel>();
        
            var loadTasks = new List<Task<List<UserModel>>>();    
        
            foreach (var userApi in _userApis)
            {
                var request = userApi.SendRequest();
                
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