using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.QuartzExternalSources.Abstract;
using Chat.Database.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Core.QuartzExternalSources.Services
{
    public class ConsistencyUserLoader : IUserLoader
    {
        private readonly IEnumerable<IUserApi> _userApis;

        public ConsistencyUserLoader(IServiceProvider serviceProvider)
        {
            _userApis = serviceProvider.GetServices(typeof(IUserApi)).Cast<IUserApi>();
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var result = new List<UserModel>();

            foreach (var userApi in _userApis)
            {
                try
                {
                    var request = await userApi.SendRequest();
                    
                    result.AddRange(request);
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return result;
        }
    }
}